using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly Member _users;

        public Guid ID
        {
            get
            {
                return _users.ID;
            }
            set
            {
                _users.ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public string FirstName
        {
            get
            {
                return _users.FirstName;
            }
            set
            {
                _users.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get
            {
                return _users.LastName;
            }
            set
            {
                _users.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string EmailAddress
        {
            get
            {
                return _users.EmailAddress;
            }
            set
            {
                _users.EmailAddress = value;
                OnPropertyChanged(nameof(EmailAddress));
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _users.PhoneNumber;
            }
            set
            {
                _users.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public UserViewModel(Member user)
        {
            _users = user;
        }
    }
}
