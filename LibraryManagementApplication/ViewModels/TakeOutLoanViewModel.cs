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
    //define everything that take out loan needs to bind to
    public class TakeOutLoanViewModel : ViewModelBase //all view models inherit the view model base
    {
        private readonly Library _library; 

        private Book _selectedBook;

        private Member _selectedUser;

        private readonly ObservableCollection<Book> _books;

        private readonly ObservableCollection<Member> _users;

        public IEnumerable<Book> Books => _books;

        public IEnumerable<Member> Users => _users;

        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook)); //Updates on selection
            }
        }

        public Member SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser)); //Updates on selection
            }
        }

        private Guid _id;

        //these property names are what is used for binding
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

        private int _bookID;

        public int BookID
        {
            get
            {
                return _bookID;
            }
            set
            {
                _bookID = value;
                OnPropertyChanged(nameof(BookID));
            }
        }

        //username property
        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _bookTitle;

        public string BookTitle
        {
            get
            {
                return _bookTitle;
            }
            set
            {
                _bookTitle = value;
                OnPropertyChanged(nameof(BookTitle));
            }
        }

        private string _bookAuthor;

        public string BookAuthor
        {
            get
            {
                return _bookAuthor;
            }
            set
            {
                _bookAuthor = value;
                OnPropertyChanged(nameof(BookAuthor));
            }
        }

        private DateTime _startTime = DateTime.Now;

        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        private DateTime _endTime = DateTime.Now;

        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
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

        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }

        //constructor
        public TakeOutLoanViewModel(Library library, NavigationService loanListViewNavigationService)
        {
            _library = library;
            _books = new ObservableCollection<Book>();
            _users = new ObservableCollection<Member>();
            SubmitCommand = new TakeOutLoanCommand(this, library, loanListViewNavigationService);
            CancelCommand = new NavigateCommand(loanListViewNavigationService);
            UpdateBooks();
            UpdateUsers();
        }

        public void UpdateBooks()
        {
            _books.Clear();

            //loop through all books, adds to ObeservableCollection
            foreach (Book book in _library.GetAllBooks())
            {
                Book b = new Book(book.ID, book.Title, book.Author, book.Description, book.ISBN);
                _books.Add(b);
            }
        }

        public void UpdateUsers()
        {
            _users.Clear();

            //loop through all users, adds to ObeservableCollection
            foreach (Member user in _library.GetAllUsers())
            {
                Member u = new Member(user.ID, user.FirstName, user.LastName, user.EmailAddress, user.PhoneNumber);
                _users.Add(u);
            }
        }
    }
}
