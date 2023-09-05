using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.ViewModels
{
    //this is how view models glue the model and the view together
    public class BookLoanViewModel : ViewModelBase
    {
        private readonly BookLoan _bookLoan;

        //changed from Book to string, for bookID
        //can do this because not binded directly to the model

        //All these values can come from book loan field and can be binded to
        public string BookID => _bookLoan.BookID.ToString();
        public Guid ID => _bookLoan.ID;
        public Guid UserID => _bookLoan.UserID;
        public string StartTime => _bookLoan.StartTime.ToString("d");
        public string EndTime => _bookLoan.EndTime.ToString("d");
        public string Overdue => _bookLoan.Overdue.ToString();

        public Book Book => _bookLoan.Book;
        public Member Member => _bookLoan.Member;

        //get entire book loan model using constructor
        public BookLoanViewModel(BookLoan bookLoan)
        {
            _bookLoan = bookLoan;
        }
    }
}
