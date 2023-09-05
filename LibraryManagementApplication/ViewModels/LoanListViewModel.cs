using LibraryManagementApplication.Commands;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagementApplication.ViewModels
{
    public class LoanListViewModel : ViewModelBase
    {
        private readonly Library _library;

        //need to provide with with a collection of loans
        //use BookLoanViewModel instead of BookLoan
        private readonly ObservableCollection<BookLoanViewModel> _bookLoans;

        private BookLoanViewModel _selectedLoan;

        //expose as IEnumerable for encapsulation
        //Can be made ObservableCollection<BookLoanViewModel> if don't want encapsulation
        //Exposed as a property that can be binded to (BookLoans)
        public IEnumerable<BookLoanViewModel> BookLoans => _bookLoans;
        public ICommand TakeOutLoanCommand { get; } 
        public ICommand DeleteCommand { get; }
        public ICommand ReportLoanCommand { get; } 

        //Selected loan in list
        public BookLoanViewModel SelectedLoan
        {
            get => _selectedLoan;
            set
            {
                _selectedLoan = value;
                OnPropertyChanged(nameof(SelectedLoan)); //Updates on selection
            }
        }

        public LoanListViewModel(Library library, NavigationService takeOutLoanNavigationService, NavigationService refreshLoanListNavigationService)
        {

            _library = library;
            //ObservableCollections automatically updates list view
            //binded to itemsource of list view
            _bookLoans = new ObservableCollection<BookLoanViewModel>();

            TakeOutLoanCommand = new NavigateCommand(takeOutLoanNavigationService);

            DeleteCommand = new DeleteLoanCommand(this, library, refreshLoanListNavigationService);

            ReportLoanCommand = new ReportLoanCommand(this, library, refreshLoanListNavigationService);

            UpdateLoans();
        }

        private void UpdateLoans()
        {
            _bookLoans.Clear();

            foreach (BookLoan bookLoan in _library.GetAllBookLoans())
            {
                BookLoanViewModel bookLoanViewModel = new BookLoanViewModel(bookLoan);
                _bookLoans.Add(bookLoanViewModel);

            }
        }
    }
}
