using LibraryManagementApplication.Commands;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagementApplication.ViewModels
{

    //define everything that add new book needs to bind to
    public class AddNewBookViewModel : ViewModelBase //all view models inherit the view model base
    {
        //ID property
        private Guid _id;

        //these property name are what is used for binding
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

        //only set once in constructor, not instantiated yet, done in constructor
        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }

        //constructor
        //Because of navigation service, no longer needs navigation store and Func<ViewModel> passed
        public AddNewBookViewModel(Library library, NavigationService bookListViewNavigationService)
        {
            SubmitCommand = new AddNewBookCommand(this, library, bookListViewNavigationService);
            CancelCommand = new NavigateCommand(bookListViewNavigationService);
        }
    }
}
