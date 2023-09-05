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

    //This command is used for deleting loans from both the regular loan list view
    //and the overdue loan list view, there are 2 constructors because of this and 
    //if / else statements in can execute and execute to check which view model is currently being used

    public class DeleteLoanCommand : CommandBase
    {
        private readonly LoanListViewModel _loanListViewModel;
        private readonly OverdueLoanListViewModel _overdueLoanListViewModel;
        private readonly Library _library;
        private readonly NavigationService _loanListViewNavigationService;
        private readonly NavigationService _overdueLoanListNavigationService;
        private bool isOverdueLoanListViewModel = false;

        public DeleteLoanCommand(LoanListViewModel loanListViewModel, Library library, NavigationService loanListViewNavigationService)
        {
            isOverdueLoanListViewModel = false;
            _loanListViewModel = loanListViewModel;
            _library = library;
            _loanListViewNavigationService = loanListViewNavigationService;
            _loanListViewModel.PropertyChanged += OnViewModelPropertyChanged;

        }

        public DeleteLoanCommand(OverdueLoanListViewModel overdueLoanListViewModel, Library library, NavigationService overdueLoanListViewNavigationService)
        {
            isOverdueLoanListViewModel = true;
            _overdueLoanListViewModel = overdueLoanListViewModel;
            _library = library;
            _overdueLoanListNavigationService = overdueLoanListViewNavigationService;
            _overdueLoanListViewModel.PropertyChanged += OnViewModelPropertyChanged;

        }

        public override void Execute(object parameter)
        {
            if (isOverdueLoanListViewModel)
            {
                if (_overdueLoanListViewModel.SelectedLoan != null)
                {
                    try
                    {
                        //if date.now > end date delete
                        _library.DeleteLoanByID(_overdueLoanListViewModel.SelectedLoan.ID);
                        _overdueLoanListViewModel.SelectedLoan = null;
                        MessageBox.Show("Successfully Deleted The Loan.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        _overdueLoanListNavigationService.Navigate(); //navigate to refresh page
                    }
                    catch
                    {
                        MessageBox.Show("There is no loan with a matching ID to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                if (_loanListViewModel.SelectedLoan != null)
                {
                    try
                    {
                        //if date.now > end date delete
                        _library.DeleteLoanByID(_loanListViewModel.SelectedLoan.ID);
                        _loanListViewModel.SelectedLoan = null;
                        MessageBox.Show("Successfully Deleted The Loan.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        _loanListViewNavigationService.Navigate(); //navigate to refresh page
                    }
                    catch
                    {
                        MessageBox.Show("There is no loan with a matching ID to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //If the selected book != null, and timeNow later than loan start, then can execute
        //This ensures that only loans that are already out can be returned
        public override bool CanExecute(object parameter)
        {
            DateTime timeNow = DateTime.Now;

            if (isOverdueLoanListViewModel)
            {
                return _overdueLoanListViewModel.SelectedLoan != null //if a loan is selected
                && timeNow >= DateTime.Parse(_overdueLoanListViewModel.SelectedLoan.StartTime); //if the time now is >= start date
            }
            else
            {
                return _loanListViewModel.SelectedLoan != null //if a loan is selected
                && timeNow >= DateTime.Parse(_loanListViewModel.SelectedLoan.StartTime); //if the time now is >= start date
            }


        }

        //If loan is selected
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoanListViewModel.SelectedLoan))
            {
                OnCanExecutedChanged();
            }
        }
    }
}

