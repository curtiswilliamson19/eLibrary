using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.ViewModels;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementApplication.Commands
{
    public class AddNewBookCommand : CommandBase
    {
        private readonly AddNewBookViewModel _addNewBookViewModel;
        private readonly Library _library;
        private readonly NavigationService _bookListViewNavigationService;

        //data passed in view model
        public AddNewBookCommand(AddNewBookViewModel addNewBookViewModel, Library library, NavigationService bookListViewNavigationService)
        {
            _addNewBookViewModel = addNewBookViewModel;
            _library = library;
            _bookListViewNavigationService = bookListViewNavigationService;
            _addNewBookViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_addNewBookViewModel.Title) &&
                !string.IsNullOrEmpty(_addNewBookViewModel.Author) &&
                !string.IsNullOrEmpty(_addNewBookViewModel.ISBN) &&

                base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {

            Book book = new Book(_addNewBookViewModel.ID, _addNewBookViewModel.Title,
                _addNewBookViewModel.Author, _addNewBookViewModel.Description, _addNewBookViewModel.ISBN);

            try
            {
                _library.AddBook(book);
                MessageBox.Show("Successfully added a book.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


                _bookListViewNavigationService.Navigate(); //on book add, go to book list
            }
            catch (BookConflictException)
            {
                MessageBox.Show("This book has a matching ID!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddNewBookViewModel.Title) ||
                e.PropertyName == nameof(AddNewBookViewModel.Author) ||
                e.PropertyName == nameof(AddNewBookViewModel.ISBN))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
