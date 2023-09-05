using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    class UserEditHasLoanException : Exception
    {
        public Member user { get; }

        public UserEditHasLoanException() { }
    }
}
