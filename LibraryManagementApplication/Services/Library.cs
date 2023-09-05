using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services.Interfaces;
using LibraryManagementApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//THIS IS MORE OF A SERVICE THAN A MODEL -----------------------------------------------------------
namespace LibraryManagementApplication.Services
{
    public class Library : ILibraryService
    {
        private readonly BookLoans _bookLoan;

        private readonly Books _books;

        private readonly Members _users;

        public string Name { get; }

        public Library(string name)
        {
            Name = name;
            _bookLoan = new BookLoans();
            _books = new Books();
            _users = new Members();
            
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _books.GetAllBooks();
        }

        public IEnumerable<BookLoan> GetAllBookLoans()
        {
            return _bookLoan.GetAllBookLoansWithTitle();
        }

        public IEnumerable<BookLoan> GetAllOverdueBookLoans()
        {
            return _bookLoan.GetAllOverdueBookLoans();
        }

        internal IEnumerable<Member> GetAllUsers()
        {
            return _users.GetAllUsers();
        }

        public IEnumerable<Book> GetAllPopularBooks()
        {
            return _books.GetAllPopularBooks();
        }

        //book
        public void AddBook(Book book)
        {
            _books.AddBook(book);
        }

        public void DeleteBookByID(Guid bookID)
        {
            _books.DeleteBookByID(bookID);
        }

        public void EditBook(Book book)
        {
            _books.EditBook(book);
        }

        //loan
        public void AddBookLoan(BookLoan bookLoan)
        {
            _bookLoan.AddBookLoan(bookLoan);
        }

        public void DeleteLoanByID(Guid loanID)
        {
            _bookLoan.DeleteLoanByID(loanID);
        }

        public void ReportLoanByID(Guid loanID)
        {
            _bookLoan.ReportLoanByID(loanID);
        }

        //user
        public void AddUser(Member user)
        {
            _users.AddUser(user);
        }

        public void DeleteUserByID(Guid userID)
        {
            _users.DeleteUserByID(userID);
        }

        public void EditUser(Member user)
        {
            _users.EditUser(user);
        }
    }
}
