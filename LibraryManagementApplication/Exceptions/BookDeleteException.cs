using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    class BookDeleteException : Exception
    {
        public Guid NonExistentBookID { get; }

        public BookDeleteException() { }
    }
}
