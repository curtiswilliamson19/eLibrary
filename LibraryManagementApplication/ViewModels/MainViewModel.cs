using LibraryManagementApplication.Commands;
using LibraryManagementApplication.Database;
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
    //this provides bindings for the main window
    //resposible for displaying the current view model
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore, NavigationService addNewBookNavigationService, 
            NavigationService bookListViewNavigationService, NavigationService takeOutLoanNavigationService,
            NavigationService loanListViewNavigationService, NavigationService addNewUserNavigationService,
            NavigationService addUserNavigationService, 
            NavigationService popularBookListViewNavigationService, NavigationService overdueLoanListViewNavigationService)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            AddNewBookNavCommand = new NavigateCommand(addNewBookNavigationService);
            ViewBookListNavCommand = new NavigateCommand(bookListViewNavigationService);
            TakeOutLoanNavCommand = new NavigateCommand(takeOutLoanNavigationService);
            LoanListNavCommand = new NavigateCommand(loanListViewNavigationService);
            AddNewUserNavCommand = new NavigateCommand(addNewUserNavigationService);
            ViewUserListNavCommand = new NavigateCommand(addUserNavigationService);
            PopularBooksNavCommand = new NavigateCommand(popularBookListViewNavigationService);
            OverdueLoansNavCommand = new NavigateCommand(overdueLoanListViewNavigationService);
            ExitCommand = new ExitCommand();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ICommand AddNewBookNavCommand { get; set; }

        public ICommand ViewBookListNavCommand { get; }

        public ICommand TakeOutLoanNavCommand { get; }

        public ICommand LoanListNavCommand { get; }

        public ICommand AddNewUserNavCommand { get; set; }

        public ICommand ViewUserListNavCommand { get; set; }

        public ICommand PopularBooksNavCommand { get; }

        public ICommand OverdueLoansNavCommand { get; }

        public ICommand ExitCommand { get; }
    }
}
