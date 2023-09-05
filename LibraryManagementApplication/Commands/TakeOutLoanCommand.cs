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
    public class TakeOutLoanCommand : CommandBase
    {
        private readonly TakeOutLoanViewModel _takeOutLoanViewModel;
        private readonly Library _library;
        private readonly NavigationService _loanListViewNavigationService;

        //data passed in view model
        public TakeOutLoanCommand(TakeOutLoanViewModel takeOutLoanViewModel, Library library, NavigationService loanListViewNavigationService)
        {
            _takeOutLoanViewModel = takeOutLoanViewModel;
            _library = library;
            _loanListViewNavigationService = loanListViewNavigationService;
            _takeOutLoanViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        //If book && user != null, can try to execute
        public override bool CanExecute(object parameter)
        {
            return _takeOutLoanViewModel.SelectedBook != null &&
                _takeOutLoanViewModel.SelectedUser != null &&
                base.CanExecute(parameter);
        }


        public override void Execute(object parameter)
        {
            //this data is in TakeOutLoan View Model, data is passed using TakeOutLoanCommand in TakeOutLoan View Model
            BookLoan bookLoan = new BookLoan(_takeOutLoanViewModel.ID, _takeOutLoanViewModel.SelectedBook, _takeOutLoanViewModel.SelectedUser, 
                _takeOutLoanViewModel.SelectedBook.ID, _takeOutLoanViewModel.SelectedUser.ID, _takeOutLoanViewModel.StartTime, _takeOutLoanViewModel.EndTime);

            try
            {
                _library.AddBookLoan(bookLoan);
                MessageBox.Show("Successfully took out a new loan.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _loanListViewNavigationService.Navigate();
            }
            catch (LoanConflictException)
            {
                MessageBox.Show("Sorry, there are no more available loans of this book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(LoanDeleteException)
            {
                MessageBox.Show("Sorry, the loan start date must be before, or the same as, the end date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TakeOutLoanViewModel.BookTitle) ||
                e.PropertyName == nameof(TakeOutLoanViewModel.BookAuthor) ||
                e.PropertyName == nameof(TakeOutLoanViewModel.ISBN) ||
                e.PropertyName == nameof(TakeOutLoanViewModel.Username) ||
                e.PropertyName == nameof(TakeOutLoanViewModel.SelectedBook) ||
                e.PropertyName == nameof(TakeOutLoanViewModel.SelectedUser))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
