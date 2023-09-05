using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementApplication.Services
{
    //Data structure that will hold all books loans
    public class BookLoans : IBookLoansService
    {

        private readonly List<BookLoan> _bookLoans;

        public BookLoans()
        {
            _bookLoans = GetAllBookLoans();
        }

        //get all book loans, viewing loans
        public List<BookLoan> GetAllBookLoans()
        {
            var cfg = new Configuration();

            cfg.Configure();

            //var sessionFactory = cfg.BuildSessionFactory();
            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                ICriteria criteria = session.CreateCriteria<BookLoan>();

                List<BookLoan> bookLoansDB = (List<BookLoan>)criteria.List<BookLoan>();

                try
                {
                    session.SaveOrUpdate(bookLoansDB);

                    transaction.Commit(); //commit transaction
                }
                catch
                {
                    transaction.Rollback();
                }

                return bookLoansDB;
            }
        }

        public List<BookLoan> GetAllOverdueBookLoans()
        {
            var cfg = new Configuration();

            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {
                Book bookAlias = null;
                BookLoan bookLoanAlias = null;

                var lstWork = session.QueryOver<BookLoan>(() => bookLoanAlias)
                    .JoinEntityAlias(
                    () => bookAlias,
                    () => bookLoanAlias.BookID == bookAlias.ID)

                    .SelectList(list => list
                    .SelectGroup(l => l.ID).WithAlias(() => bookLoanAlias.ID)
                    .SelectGroup(l => l.BookID).WithAlias(() => bookLoanAlias.BookID)
                    .SelectGroup(l => l.UserID).WithAlias(() => bookLoanAlias.UserID)
                    .SelectGroup(l => l.StartTime).WithAlias(() => bookLoanAlias.StartTime)
                    .SelectGroup(l => l.EndTime).WithAlias(() => bookLoanAlias.EndTime)
                    .SelectGroup(l => l.Book).WithAlias(() => bookLoanAlias.Book)
                    .SelectGroup(l => l.Member).WithAlias(() => bookLoanAlias.Member)
                    )
                    .Where(l => l.Overdue == true)
                    .TransformUsing(Transformers.PassThrough)
                    .List<object[]>();


                var booksLoansDB = new List<BookLoan>();

                foreach (object[] result in lstWork)
                {
                    BookLoan bl = new BookLoan((Guid)result[0], (Book)result[5], (Member)result[6], (Guid)result[1], (Guid)result[2], (DateTime)result[3], (DateTime)result[4]);

                    bl.Overdue = true; //this can just be set here as all books will be overdue due to Where(l => l.Overdue == true) as not set in constructor

                    booksLoansDB.Add(bl);

                    Console.WriteLine("Result 0: " + result[0]); //ID
                    Console.WriteLine("Result 1: " + result[1]); //BookID
                    Console.WriteLine("Result 2: " + result[2]); //UserID
                    Console.WriteLine("Result 3: " + result[3]); //StartTime
                    Console.WriteLine("Result 4: " + result[4]); //EndTime
                    Console.WriteLine("Result 5: " + result[5]); //Book
                    Console.WriteLine("Result 6: " + result[6]); //User

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
                return booksLoansDB;
            }
        }

        //Making a book loan, need to check if conflicts with existing book loans
        public void AddBookLoan(BookLoan bookLoan)
        {
            var cfg = new Configuration();
            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                
                //Did 2 criteria queries here rather than QueryOver for some practice, probably easier using one QueryOver

                //This checks if the loan overlaps with another
                ICriteria criteria = session.CreateCriteria<BookLoan>();
                criteria.Add(Restrictions.Eq("BookID", bookLoan.BookID)); //bookLoanDB.BookID == bookLoan.BookID
                criteria.Add(Restrictions.Le("StartTime", bookLoan.EndTime.Date)); // booLoanDB.StartTime <= bookLoan.EndTime.Date
                criteria.Add(Restrictions.Ge("EndTime", bookLoan.StartTime.Date)); // booLoanDB.EndTime >= bookLoan.StartTime.Date

                //This checks if the book has any overdue loans, if so, can't be loaned
                ICriteria criteriaOverdue = session.CreateCriteria<BookLoan>();
                criteriaOverdue.Add(Restrictions.Eq("BookID", bookLoan.BookID)); //bookLoansOverdueDB.BookID == bookLoan.BookID
                criteriaOverdue.Add(Restrictions.Eq("Overdue", true)); //bookLoansOverdueDB.Overdue == true


                List<BookLoan> bookLoansDB = criteria.List<BookLoan>().ToList();
                List<BookLoan> bookLoansOverdueDB = criteriaOverdue.List<BookLoan>().ToList();

                try
                {
                    if (bookLoan.EndTime.Date < bookLoan.StartTime.Date)
                    {
                        throw new LoanDeleteException();
                    }

                    if (bookLoansDB.Count() > 0 || bookLoansOverdueDB.Count() > 0) //if a book in loans matches criteria, i.e. overlaps or is overdue
                    {
                        throw new LoanConflictException(bookLoan);
                    }

                    //does not store time in DB for comparison logic
                    bookLoan.StartTime = bookLoan.StartTime.Date; 
                    bookLoan.EndTime = bookLoan.EndTime.Date;

                    session.SaveOrUpdate(bookLoan);
                    transaction.Commit(); //commit transaction
                }
                catch (LoanConflictException)
                {
                    transaction.Rollback();
                    throw new LoanConflictException(bookLoan);
                }
                catch (LoanDeleteException)
                {
                    transaction.Rollback();
                    throw new LoanDeleteException();
                }
            }
        }

        //Loop through bookLoans, if book ID = given ID, delete
        //Essentially returning a loan
        //This does not need to check for date as this is checked in canExecute
        public void DeleteLoanByID(Guid loanID)
        {
            var cfg = new Configuration();

            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {
                BookLoan bookLoanDB = (BookLoan)session.Get("BookLoan", loanID); //This does not need to check for date as this is checked in canExecute

                try
                {

                    if (bookLoanDB == null)
                    {
                        throw new LoanDeleteException();
                    }
                    else
                    {
                        session.Delete(bookLoanDB);
                    }

                    transaction.Commit(); //commit transaction
                }
                catch (LoanDeleteException)
                {
                    transaction.Rollback();
                    throw new LoanDeleteException();
                }
            }
        }

        public void ReportLoanByID(Guid loanID)
        {
            var cfg = new Configuration();

            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                try
                {
                    BookLoan bookLoanDB = (BookLoan)session.Get("BookLoan", loanID); //don't need criteria as only 1?

                    if (bookLoanDB.EndTime.Date < DateTime.Now.Date)
                    {
                        bookLoanDB.Overdue = true;
                        session.SaveOrUpdate(bookLoanDB);
                    }
                    else if (bookLoanDB.EndTime.Date >= DateTime.Now.Date)
                    {
                        throw new LoanNotOverdueException();
                    }

                    transaction.Commit(); //commit transaction
                }
                catch (LoanNotOverdueException)
                {
                    transaction.Rollback();
                    throw new LoanNotOverdueException();
                }
            }
        }

        //Can't loan a book for same day, another user can't take out that book until the day after return,
        public bool Conflicts(BookLoan bookLoan, BookLoan bookLoan2)
        {
            //If not same book, false
            if (bookLoan.BookID != bookLoan2.BookID)
            {
                return false;
            }

            //if books have same ID, return true if the loan overlaps with another loan
            if (bookLoan.StartTime.Date <= bookLoan2.EndTime.Date && bookLoan.EndTime.Date >= bookLoan2.StartTime.Date)
            {
                return true;
            }

            //if books have same ID, return false if the end date <= start
            if (bookLoan.StartTime <= bookLoan.EndTime && bookLoan.EndTime >= bookLoan.StartTime &&
                bookLoan2.StartTime <= bookLoan2.EndTime && bookLoan2.EndTime >= bookLoan2.StartTime)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public List<BookLoan> GetAllBookLoansWithTitle()
        {
            var cfg = new Configuration();

            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                Book bookAlias = null;
                BookLoan bookLoanAlias = null;

                var lstWork = session.QueryOver<BookLoan>(() => bookLoanAlias)
                    .JoinEntityAlias(
                    () => bookAlias,
                    () => bookLoanAlias.BookID == bookAlias.ID)

                    .SelectList(list => list
                    .SelectGroup(l => l.ID).WithAlias(() => bookLoanAlias.ID)
                    .SelectGroup(l => l.BookID).WithAlias(() => bookLoanAlias.BookID)
                    .SelectGroup(l => l.UserID).WithAlias(() => bookLoanAlias.UserID)
                    .SelectGroup(l => l.StartTime).WithAlias(() => bookLoanAlias.StartTime)
                    .SelectGroup(l => l.EndTime).WithAlias(() => bookLoanAlias.EndTime)
                    .SelectGroup(l => l.Overdue).WithAlias(() => bookLoanAlias.Overdue)
                    .SelectGroup(l => l.Book).WithAlias(() => bookLoanAlias.Book)
                    .SelectGroup(l => l.Member).WithAlias(() => bookLoanAlias.Member)
                    )

                    .TransformUsing(Transformers.PassThrough)
                    //.TransformUsing(Transformers.AliasToBean<Book>()) //This flattens the result set

                    //.Where(() => bookLoanAlias.UserID == thisOne) //this could be used for filtering further
                    .List<object[]>();

                var booksLoansDB = new List<BookLoan>();

                foreach (object[] result in lstWork)
                {
                    
                    BookLoan bl = new BookLoan((Guid)result[0], (Book)result[6], (Member)result[7], (Guid)result[1], (Guid)result[2], (DateTime)result[3], (DateTime)result[4]);

                    if((bool)result[5] == true)
                    {
                        bl.Overdue = true;
                    }

                    booksLoansDB.Add(bl);

                    Console.WriteLine("Result 0: " + result[0]); //ID
                    Console.WriteLine("Result 1: " + result[1]); //BookID
                    Console.WriteLine("Result 2: " + result[2]); //UserID
                    Console.WriteLine("Result 3: " + result[3]); //StartTime
                    Console.WriteLine("Result 4: " + result[4]); //EndTime
                    Console.WriteLine("Result 5: " + result[5]); //OverDue
                    Console.WriteLine("Result 6: " + result[6]); //Book
                    Console.WriteLine("Result 7: " + result[7]); //User

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
                return booksLoansDB;
            }
        }

        //Example Using criteria query

        //public List<BookLoan> GetAllOverdueBookLoans()
        //{
        //    var cfg = new Configuration();

        //    cfg.Configure();

        //    //var sessionFactory = cfg.BuildSessionFactory();
        //    using (ISession session = cfg.BuildSessionFactory().OpenSession())

        //    using (ITransaction transaction = session.BeginTransaction())
        //    {

        //        ICriteria criteria = session.CreateCriteria<BookLoan>();

        //        criteria.Add(Restrictions.Eq("Overdue", true));

        //        List<BookLoan> bookLoansDB = (List<BookLoan>)criteria.List<BookLoan>();

        //        try
        //        {
        //            //session.SaveOrUpdate(bookLoansDB);

        //            transaction.Commit(); //commit transaction
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //            transaction.Rollback();
        //        }

        //        return bookLoansDB;
        //    }
        //}

    }
}
