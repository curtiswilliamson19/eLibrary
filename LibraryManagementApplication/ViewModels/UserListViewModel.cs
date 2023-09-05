using LibraryManagementApplication.Commands;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagementApplication.ViewModels
{
    public class UserListViewModel : ViewModelBase
    {
        private readonly Library _library;


        private readonly ObservableCollection<UserViewModel> _users; 


        private UserViewModel _selectedUser;

        //expose as IEnumerable for encapsulation
        //Can be made ObservableCollection<UserViewModel> if don't want encapsulation
        //Exposed as a property that can be binded to
        public IEnumerable<UserViewModel> Users => _users; 
        public ICommand AddNewUserCommand { get; } 
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }


        public UserViewModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser)); //Updates on selection
            }

        }

        public UserListViewModel(Library library, NavigationService addNewUserNavigationService, NavigationService refreshUserListNavigationService)
        {
            _library = library;
            _users = new ObservableCollection<UserViewModel>();
            AddNewUserCommand = new NavigateCommand(addNewUserNavigationService);
            DeleteCommand = new DeleteUserCommand(this, library, refreshUserListNavigationService);
            EditCommand = new EditUserCommand(this, library, refreshUserListNavigationService);
            UpdateUsers();
        }


        public void UpdateUsers()
        {
            _users.Clear();

            //loop through all users, map each to a user view model
            foreach (Member user in _library.GetAllUsers())
            {
                UserViewModel userViewModel = new UserViewModel(user);
                _users.Add(userViewModel);
            }
        }










        ////This info is needed for editing users
        //private Guid _id;
        //public Guid ID
        //{
        //    get
        //    {
        //        return _id;
        //    }
        //    set
        //    {
        //        _id = value;
        //        OnPropertyChanged(nameof(ID));
        //    }
        //}

        //private string _firstName;
        //public string FirstName
        //{
        //    get
        //    {
        //        return _firstName;
        //    }
        //    set
        //    {
        //        _firstName = value;
        //        OnPropertyChanged(nameof(FirstName));
        //    }
        //}

        //private string _lastName;
        //public string LastName
        //{
        //    get
        //    {
        //        return _lastName;
        //    }
        //    set
        //    {
        //        _lastName = value;
        //        OnPropertyChanged(nameof(LastName));
        //    }
        //}

        //private string _emailAddress;
        //public string EmailAddress
        //{
        //    get
        //    {
        //        return _emailAddress;
        //    }
        //    set
        //    {
        //        _emailAddress = value;
        //        OnPropertyChanged(nameof(EmailAddress));
        //    }
        //}

        //private string _phoneNumber;
        //public string PhoneNumber
        //{
        //    get
        //    {
        //        return _phoneNumber;
        //    }
        //    set
        //    {
        //        _phoneNumber = value;
        //        OnPropertyChanged(nameof(PhoneNumber));
        //    }
        //}

    }
}
