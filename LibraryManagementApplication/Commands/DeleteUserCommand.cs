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
    public class DeleteUserCommand : CommandBase
    {
        private readonly UserListViewModel _userListViewModel;
        private readonly Library _library;
        private readonly NavigationService _userListViewNavigationService;

        public DeleteUserCommand(UserListViewModel userListViewModel, Library library, NavigationService userListViewNavigationService)
        {
            _userListViewModel = userListViewModel;
            _library = library;
            _userListViewNavigationService = userListViewNavigationService;
            _userListViewModel.PropertyChanged += OnViewModelPropertyChanged;
           
        }

        public override void Execute(object parameter)
        {
            if (_userListViewModel.SelectedUser != null)
            {
                try
                {
                    _library.DeleteUserByID(_userListViewModel.SelectedUser.ID);
                    _userListViewModel.SelectedUser = null;
                    MessageBox.Show("Successfully Deleted The User.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _userListViewNavigationService.Navigate(); //navigate to refresh page

                }
                catch (UserEditHasLoanException)
                {
                    MessageBox.Show("This user has an active loan!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _userListViewNavigationService.Navigate(); //navigate to refresh page
                }
                catch
                {
                    MessageBox.Show("There is no user with a matching ID to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _userListViewNavigationService.Navigate(); //navigate to refresh page
                }

            }
        }

        //If the selected user != null, then can try to execute
        public override bool CanExecute(object parameter)
        {
            return _userListViewModel.SelectedUser != null;
        }

        //If user is selected, then can delete
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserListViewModel.SelectedUser))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
