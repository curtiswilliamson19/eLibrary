using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    class BookConflictException : Exception
    {
        public Book ExistingBook { get; }

        public Book IncomingBook { get; }

        public BookConflictException(Book incomingBook)
        {
            IncomingBook = incomingBook;
        }
        public BookConflictException(Book existingBook, Book incomingBook)
        {
            ExistingBook = existingBook;
            IncomingBook = incomingBook;
        }

        public BookConflictException(string message, Book existingBook, Book incomingBook) : base(message)
        {
            ExistingBook = existingBook;
            IncomingBook = incomingBook;
        }

        public BookConflictException(string message, Exception innerException, Book existingBook, Book incomingBook) : base(message, innerException)
        {
            ExistingBook = existingBook;
            IncomingBook = incomingBook;
        }
    }
}
