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
    public class EditUserCommand : CommandBase
    {
        private readonly UserListViewModel _userListViewModel;
        private readonly Library _library;
        private readonly NavigationService _userListViewNavigationService;

        //data passed in view model
        public EditUserCommand(UserListViewModel userListViewModel, Library library, NavigationService userListViewNavigationService)
        {
            _userListViewModel = userListViewModel;
            _library = library;
            _userListViewNavigationService = userListViewNavigationService;
            _userListViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        //If the selected user && first && last && email && phone != null, can try to execute
        public override bool CanExecute(object parameter)
        {
            return _userListViewModel.SelectedUser != null &&
                !string.IsNullOrEmpty(_userListViewModel.SelectedUser.FirstName) &&
                !string.IsNullOrEmpty(_userListViewModel.SelectedUser.LastName) &&
                !string.IsNullOrEmpty(_userListViewModel.SelectedUser.EmailAddress) &&
                !string.IsNullOrEmpty(_userListViewModel.SelectedUser.PhoneNumber) &&

                base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            Member user = new Member(_userListViewModel.SelectedUser.ID, _userListViewModel.SelectedUser.FirstName,
                _userListViewModel.SelectedUser.LastName, _userListViewModel.SelectedUser.EmailAddress, _userListViewModel.SelectedUser.PhoneNumber);

            try
            {
                _library.EditUser(user);
                MessageBox.Show("Successfully Edited a user.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _userListViewNavigationService.Navigate(); //on book add, go to book list
            }
            catch (UserEditHasLoanException)
            {
                MessageBox.Show("This user can't be edited as they have an active loan!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _userListViewNavigationService.Navigate();
            }
            catch(UserConflictException)
            {
                MessageBox.Show("This user email already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _userListViewNavigationService.Navigate();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserListViewModel.SelectedUser) ||
                e.PropertyName == nameof(UserListViewModel.SelectedUser.FirstName) ||
                e.PropertyName == nameof(UserListViewModel.SelectedUser.LastName) ||
                e.PropertyName == nameof(UserListViewModel.SelectedUser.EmailAddress) ||
                e.PropertyName == nameof(UserListViewModel.SelectedUser.PhoneNumber)) 
            {
                OnCanExecutedChanged();
            }
        }
    }
}
