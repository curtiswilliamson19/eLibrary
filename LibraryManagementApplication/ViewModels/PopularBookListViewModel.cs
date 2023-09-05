using LibraryManagementApplication.Commands;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagementApplication.ViewModels
{

    public class PopularBookListViewModel : ViewModelBase
    {

        private readonly Library _library;

        private readonly ObservableCollection<BookViewModel> _books; 

        private BookViewModel _selectedBook;

        public IEnumerable<BookViewModel> Books => _books;


        public BookViewModel SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook)); //Updates on selection
            }

        }

        public PopularBookListViewModel(Library library, NavigationService refreshBookListNavigationService) //May not need navigation
        {
            _library = library;

            //ObservableCollections automatically updates list view
            //binded to itemsource of list view
            _books = new ObservableCollection<BookViewModel>();

            UpdateBooks();
        }

        public void UpdateBooks()
        {
            _books.Clear();

            //loop through all books, map each to a book view model
            foreach (Book book in _library.GetAllPopularBooks())
            {
                BookViewModel bookViewModel = new BookViewModel(book);
                _books.Add(bookViewModel);

            }
        }
    }
}
