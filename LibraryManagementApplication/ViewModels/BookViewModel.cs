using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Edited to allow for editing books, allows values to be get and set

namespace LibraryManagementApplication.ViewModels
{
    //this is used for displaying books
    public class BookViewModel : ViewModelBase
    {
        private readonly Book _books;

        public Guid ID
        {
            get
            {
                return _books.ID;
            }
            set
            {
                _books.ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        public string Title
        {
            get
            {
                return _books.Title;
            }
            set
            {
                _books.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string Author
        {
            get
            {
                return _books.Author;
            }
            set
            {
                _books.Author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        public string Description
        {
            get
            {
                return _books.Description;
            }
            set
            {
                _books.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string ISBN
        {
            get
            {
                return _books.ISBN;
            }
            set
            {
                _books.ISBN = value;
                OnPropertyChanged(nameof(ISBN));
            }
        }

        //get entire book model using constructor
        public BookViewModel(Book book)
        {
            _books = book;
        }
    }
}
