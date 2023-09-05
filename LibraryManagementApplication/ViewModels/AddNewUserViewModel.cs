using LibraryManagementApplication.Commands;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagementApplication.ViewModels
{
    public class AddNewUserViewModel : ViewModelBase
    {

        private Guid _id;
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

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                _emailAddress = value;
                OnPropertyChanged(nameof(EmailAddress));
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }

        public AddNewUserViewModel(Library library, NavigationService userListViewNavigationService)
        {
            SubmitCommand = new AddNewUserCommand(this, library, userListViewNavigationService);
            CancelCommand = new NavigateCommand(userListViewNavigationService);
        }

    }
}
