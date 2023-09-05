using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementApplication.Commands
{
    public class AddNewUserCommand : CommandBase
    {
        private readonly AddNewUserViewModel _addNewUserViewModel;
        private readonly Library _library;
        private readonly NavigationService _userListViewNavigationService;

        //data passed in view model
        public AddNewUserCommand(AddNewUserViewModel addNewUserViewModel, Library library, NavigationService userListViewNavigationService)
        {
            _addNewUserViewModel = addNewUserViewModel;
            _library = library;
            _userListViewNavigationService = userListViewNavigationService;
            _addNewUserViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            //can add logic in here e.g. _addNewUserViewModel.ID > 0 etc.
            return !string.IsNullOrEmpty(_addNewUserViewModel.FirstName) &&
                !string.IsNullOrEmpty(_addNewUserViewModel.LastName) &&
                !string.IsNullOrEmpty(_addNewUserViewModel.EmailAddress) &&
                !string.IsNullOrEmpty(_addNewUserViewModel.PhoneNumber) &&

                base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            Member user = new Member(_addNewUserViewModel.ID, _addNewUserViewModel.FirstName,
                _addNewUserViewModel.LastName, _addNewUserViewModel.EmailAddress, _addNewUserViewModel.PhoneNumber);

            try
            {
                _library.AddUser(user);
                MessageBox.Show("Successfully added a user.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


                _userListViewNavigationService.Navigate(); //on book add, go to book list
            }
            catch (UserConflictException)
            {
                MessageBox.Show("This user has a matching Email!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddNewUserViewModel.FirstName) ||
                e.PropertyName == nameof(AddNewUserViewModel.LastName) ||
                e.PropertyName == nameof(AddNewUserViewModel.EmailAddress) ||
                e.PropertyName == nameof(AddNewUserViewModel.PhoneNumber)) 
            {
                OnCanExecutedChanged();
            }
        }
    }
}
