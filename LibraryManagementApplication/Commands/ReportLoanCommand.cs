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
    public class ReportLoanCommand : CommandBase
    {
        private readonly LoanListViewModel _loanListViewModel;
        private readonly Library _library;
        private readonly NavigationService _loanListViewNavigationService;

        public ReportLoanCommand(LoanListViewModel loanListViewModel, Library library, NavigationService loanListViewNavigationService)
        {
            _loanListViewModel = loanListViewModel;
            _library = library;
            _loanListViewNavigationService = loanListViewNavigationService;
            _loanListViewModel.PropertyChanged += OnViewModelPropertyChanged;

        }

        public override void Execute(object parameter)
        {
            try
            {
                _library.ReportLoanByID(_loanListViewModel.SelectedLoan.ID);
                MessageBox.Show("Successfully Reported The Loan.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _loanListViewNavigationService.Navigate(); //navigate to refresh page

            }
            catch (LoanNotOverdueException)
            {
                MessageBox.Show("This loan is not overdue!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //If the selected loan != null, then can try to execute
        public override bool CanExecute(object parameter)
        {
            return _loanListViewModel.SelectedLoan != null; //if loan selected
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoanListViewModel.SelectedLoan))
            {
                OnCanExecutedChanged();
            }
        }
    }
}

