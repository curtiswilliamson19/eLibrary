using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Services
{

    public class Books : IBooksService
    {

        private readonly List<Book> _books;

        public Books()
        {
            _books = GetAllBooks();
        }

        public List<Book> GetAllBooks()
        {
            var cfg = new Configuration();

            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                ICriteria criteria = session.CreateCriteria<Book>();

                List<Book> booksDB = (List<Book>)criteria.List<Book>();

                try
                {
                    session.SaveOrUpdate(booksDB);

                    transaction.Commit(); //commit transaction
                }
                catch
                {
                    transaction.Rollback();
                }

                return booksDB;
            }
        }

        public List<Book> GetAllPopularBooks()
        {
            var cfg = new Configuration();

            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                Book bookAlias = null;
                BookLoan bookLoanAlias = null;

                var lstWork = session.QueryOver<Book>(() => bookAlias)
                    .JoinEntityAlias(
                    () => bookLoanAlias,
                    () => bookLoanAlias.BookID == bookAlias.ID)
                    .SelectList(list => list
                    .SelectGroup(b => b.ID).WithAlias(() => bookAlias.ID)
                    .SelectGroup(b => b.Title).WithAlias(() => bookAlias.Title)
                    .SelectGroup(b => b.Author).WithAlias(() => bookAlias.Author)
                    .SelectGroup(b => b.Description).WithAlias(() => bookAlias.Description)
                    .SelectGroup(b => b.ISBN).WithAlias(() => bookAlias.ISBN)
                    .SelectCount(b => b.ID)
                    )
                    .OrderBy(Projections.RowCount()).Desc


                    .TransformUsing(Transformers.PassThrough)
                    //.TransformUsing(Transformers.AliasToBean<Book>()) //This flattens the result set
                    //.Where(() => bookLoanAlias.UserID == thisOne) //this could be used for filtering further
                    .List<object[]>();

                var booksDB = new List<Book>();

                //these are already in order of count
                foreach (object[] result in lstWork)
                {
                    //concats title with count
                    string titleCount = result[1].ToString() + "\t(" + result[5].ToString() +")";

                    Book b = new Book((Guid)result[0], titleCount, (string)result[2], (string)result[3], (string)result[4]);

                    booksDB.Add(b);

                    Console.WriteLine("Result 1: " + result[0]); //ID
                    Console.WriteLine("Result 2: " + result[1]); //Title
                    Console.WriteLine("Result 3: " + result[2]); //Author
                    Console.WriteLine("Result 4: " + result[3]); //Desc
                    Console.WriteLine("Result 5: " + result[4]); //ISBN
                    Console.WriteLine("Result 0: " + result[5]); //Count

                }

                try
                {
                    //No need to save to DB here, simply display return books for display
                    transaction.Commit(); //commit transaction
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                }
                return booksDB;
            }
        }

        public void AddBook(Book book)
        {

            var cfg = new Configuration();
            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                try
                {
                    Book bookDB = (Book)session.Get("Book", book.ID);

                    if (bookDB != null) //if there is no book with matching ID, save to DB
                    {
                        throw new BookConflictException(book);
                    }
                    session.SaveOrUpdate(book);
                    transaction.Commit(); //commit transaction

                }
                catch
                {
                    transaction.Rollback();
                    throw new BookConflictException(book);
                }
            }
        }

        public void DeleteBookByID(Guid bookID)
        {
            var cfg = new Configuration();
            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                ICriteria criteria = session.CreateCriteria<BookLoan>();

                criteria.Add(Restrictions.Eq("BookID", bookID));

                List<BookLoan> bookLoansDB = criteria.List<BookLoan>().ToList();

                Book bookDB = (Book)session.Get("Book", bookID); //used this to get BookObject using passed bookID

                try
                {

                    if (bookLoansDB.Count == 0) //if not out for loan, delete book
                    {
                        session.Delete(bookDB);
                    }
                    else if (bookLoansDB.Count() > 0)
                    {
                        throw new BookDeleteOutForLoanException();
                    }

                    transaction.Commit(); //commit transaction
                }
                catch (BookDeleteOutForLoanException)
                {
                    transaction.Rollback();
                    throw new BookDeleteOutForLoanException();
                }
            }
        }

        public void EditBook(Book book)
        {
            var cfg = new Configuration();
            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                ICriteria criteria = session.CreateCriteria<BookLoan>();

                criteria.Add(Restrictions.Eq("BookID", book.ID));

                List<BookLoan> bookLoansDB = criteria.List<BookLoan>().ToList();

                try
                {
                    if (bookLoansDB.Count == 0) //if not out for loan, edit book
                    {
                        session.SaveOrUpdate(book);
                    }
                    else if (bookLoansDB.Count() > 0)
                    {
                        throw new BookEditOutForLoanException();
                    }

                    transaction.Commit(); //commit transaction
                }
                catch (BookEditOutForLoanException)
                {
                    transaction.Rollback();
                    throw new BookEditOutForLoanException();
                }
            }
        }

    }
}
