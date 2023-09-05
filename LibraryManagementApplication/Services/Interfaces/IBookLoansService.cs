using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Services
{
    interface IBookLoansService
    {
        List<BookLoan> GetAllBookLoans();

        List<BookLoan> GetAllOverdueBookLoans();

        void AddBookLoan(BookLoan bookLoan);

        void DeleteLoanByID(Guid loanID);

        void ReportLoanByID(Guid loanID); 

        bool Conflicts(BookLoan bookLoan, BookLoan bookLoan2);

        List<BookLoan> GetAllBookLoansWithTitle();
    }
}
