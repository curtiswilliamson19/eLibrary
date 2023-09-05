using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Model
{
    public class BookLoan
    {

        public virtual Guid ID { get; set; }
        public virtual Guid BookID { get; set; }
        public virtual Guid UserID { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }

        public virtual TimeSpan Length => EndTime.Subtract(StartTime);

        public virtual bool Overdue { get; set; }

        public virtual Book Book { get; set; }

        public virtual Member Member { get; set; } //has to be called member as reserved SQL word

        public BookLoan() { }

        public BookLoan(Guid iD, Book book, Member user, Guid bookID, Guid userID, DateTime startTime, DateTime endTime)
        {
            ID = iD;
            Book = book;
            Member = user;
            UserID = userID;
            BookID = bookID;
            StartTime = startTime;
            EndTime = endTime;
            Overdue = false;
        }

        ////Business logic, put in view model
    }
}
