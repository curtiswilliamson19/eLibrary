using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Services.Interfaces
{
    interface ILibraryService
    {
        void AddBookLoan(BookLoan bookLoan);

        void AddBook(Book book);

        void DeleteBookByID(Guid bookID);

        void DeleteLoanByID(Guid loanID);

        void ReportLoanByID(Guid loanID);

        void EditBook(Book book);

        void EditUser(Member user);

        void AddUser(Member user);

        void DeleteUserByID(Guid userID);
    }
}
