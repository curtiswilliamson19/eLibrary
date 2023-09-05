using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    class BookDeleteOutForLoanException : Exception
    {
        public Book Book { get; }

        public BookDeleteOutForLoanException() { }
    }
}
