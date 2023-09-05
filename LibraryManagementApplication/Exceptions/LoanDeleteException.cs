using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    class LoanDeleteException : Exception
    {
        public Guid loanID { get; }

        public LoanDeleteException() { }
    }
}
