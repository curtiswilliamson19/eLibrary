using LibraryManagementApplication.Database;
using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.Stores;
using LibraryManagementApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Library _library;
        private readonly NavigationStore _navigationStore; //single navigation store


        public App()
        {
            //DB CONNECTION
            Connection c = new Connection();
            c.Connect();

            _library = new Library("McClay Library");
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            _navigationStore.CurrentViewModel = new BookListViewModel(_library,
                new NavigationService(_navigationStore, CreateAddNewBookViewModel), //2 services needed here
                new NavigationService(_navigationStore, CreateAddBookViewModel));

            MainWindow = new MainWindow()
            {
                //DataContext = new MainViewModel(_library)
                DataContext = new MainViewModel(_navigationStore, 
                new NavigationService(_navigationStore, CreateAddNewBookViewModel), 
                new NavigationService(_navigationStore, CreateAddBookViewModel),
                new NavigationService(_navigationStore, CreateTakeOutLoanViewModel),
                new NavigationService(_navigationStore, CreateLoanViewModel),
                new NavigationService(_navigationStore, CreateAddNewUserViewModel), //add user
                new NavigationService(_navigationStore, CreateAddUserViewModel),
                new NavigationService(_navigationStore, CreatePopularBookViewModel),
                new NavigationService(_navigationStore, CreateOverdueLoanViewModel)
                )
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        //Instantiating a new view model each time, though this is recommended
        private AddNewBookViewModel CreateAddNewBookViewModel()
        {
            return new AddNewBookViewModel(_library, new NavigationService(_navigationStore, CreateAddBookViewModel));
        }

        //2 Navigation services here as one needed for going to new book and other for refreshing list
        private BookListViewModel CreateAddBookViewModel()
        {
            return new BookListViewModel(_library, 
                new NavigationService(_navigationStore, CreateAddNewBookViewModel), 
                new NavigationService(_navigationStore, CreateAddBookViewModel)); //list
        }
        
        private TakeOutLoanViewModel CreateTakeOutLoanViewModel()
        {
            return new TakeOutLoanViewModel(_library, new NavigationService(_navigationStore, CreateLoanViewModel));
        }

        //2 Navigation services here as one needed for going to new loan and other for refreshing list
        private LoanListViewModel CreateLoanViewModel()
        {
            return new LoanListViewModel(_library, 
                new NavigationService(_navigationStore, CreateTakeOutLoanViewModel),
                new NavigationService(_navigationStore, CreateLoanViewModel)); //list
        }

        private AddNewUserViewModel CreateAddNewUserViewModel()
        {
            return new AddNewUserViewModel(_library, new NavigationService(_navigationStore, CreateAddUserViewModel));
        }

        //2 Navigation services here as one needed for going to new book and other for refreshing list
        private UserListViewModel CreateAddUserViewModel()
        {
            return new UserListViewModel(_library,
                new NavigationService(_navigationStore, CreateAddNewUserViewModel),
                 new NavigationService(_navigationStore, CreateAddUserViewModel)); //list
        }


        private PopularBookListViewModel CreatePopularBookViewModel()
        {
            return new PopularBookListViewModel(_library,
                new NavigationService(_navigationStore, CreatePopularBookViewModel)); //list
        }

        private OverdueLoanListViewModel CreateOverdueLoanViewModel()
        {
            return new OverdueLoanListViewModel(_library,
                new NavigationService(_navigationStore, CreateOverdueLoanViewModel)); //list
        }

    }
}
