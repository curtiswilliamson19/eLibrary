using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    public class LoanConflictException : Exception
    {
        public BookLoan ExistingLoan { get; }

        public BookLoan IncomingLoan { get; }

        public LoanConflictException(BookLoan incomingLoan)
        {
            IncomingLoan = incomingLoan;
        }

        public LoanConflictException(BookLoan existingLoan, BookLoan incomingLoan)
        {
            ExistingLoan = existingLoan;
            IncomingLoan = incomingLoan;
        }

        public LoanConflictException(string message, BookLoan existingLoan, BookLoan incomingLoan) : base(message)
        {
            ExistingLoan = existingLoan;
            IncomingLoan = incomingLoan;
        }

        public LoanConflictException(string message, Exception innerException, BookLoan existingLoan, BookLoan incomingLoan) : base(message, innerException)
        {
            ExistingLoan = existingLoan;
            IncomingLoan = incomingLoan;
        }
    }
}
