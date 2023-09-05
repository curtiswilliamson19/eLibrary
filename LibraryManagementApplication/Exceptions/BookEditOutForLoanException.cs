using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    class BookEditOutForLoanException : Exception
    {
        public Book book { get; }

        public BookEditOutForLoanException() { }
    }
}
