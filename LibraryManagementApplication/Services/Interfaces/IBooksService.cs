using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Services
{
    interface IBooksService
    {
        void AddBook(Book book);

        void DeleteBookByID(Guid bookID);

        void EditBook(Book book);

        List<Book> GetAllBooks();

        List<Book> GetAllPopularBooks();
    }
}
