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

    public class BookListViewModel : ViewModelBase
    {
        private readonly Library _library;

        //need to provide with with a collection of books
        //use BookViewModel instead of Book
        private readonly ObservableCollection<BookViewModel> _books;

        private BookViewModel _selectedBook; //data prov

        //expose as IEnumerable for encapsulation
        //Can be made ObservableCollection<BookLoanViewModel> if don't want encapsulation
        //Exposed as a property that can be binded to
        public IEnumerable<BookViewModel> Books => _books; //ObservableCollection
        public ICommand AddNewBookCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        //Data prov
        public BookViewModel SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook)); //Updates on selection
            }
        }

        public BookListViewModel(Library library, NavigationService addNewBookNavigationService, NavigationService refreshBookListNavigationService)
        {

            _library = library;

            //ObservableCollections automatically updates list view
            //binded to itemsource of list view
            _books = new ObservableCollection<BookViewModel>();

            AddNewBookCommand = new NavigateCommand(addNewBookNavigationService);

            DeleteCommand = new DeleteBookCommand(this, library, refreshBookListNavigationService);

            EditCommand = new EditBookCommand(this, library, refreshBookListNavigationService);

            UpdateBooks();
        }

        public void UpdateBooks()
        {
            _books.Clear();

            //loop through all books, map each to a book view model
            foreach (Book book in _library.GetAllBooks())
            {
                BookViewModel bookViewModel = new BookViewModel(book);
                _books.Add(bookViewModel);

            }
        }

        //Needed to edit books in list
        private Guid _id;

        //this property name is what is used for binding
        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _author;
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _ISBN;
        public string ISBN
        {
            get
            {
                return _ISBN;
            }
            set
            {
                _ISBN = value;
                OnPropertyChanged(nameof(ISBN));
            }
        }
    }
}
