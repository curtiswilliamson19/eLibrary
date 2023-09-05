using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.Stores;
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
    public class EditBookCommand : CommandBase
    {

        private readonly BookListViewModel _bookListViewModel;
        private readonly Library _library;
        private readonly NavigationService _bookListViewNavigationService;

        public EditBookCommand(BookListViewModel bookListViewModel, Library library, NavigationService bookListViewNavigationService)
        {
            _bookListViewModel = bookListViewModel;
            _library = library;
            _bookListViewNavigationService = bookListViewNavigationService;
            _bookListViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        //If the selected book && title && author && ISBN != null, can try to execute
        public override bool CanExecute(object parameter)
        {
            return _bookListViewModel.SelectedBook != null &&
                !string.IsNullOrEmpty(_bookListViewModel.SelectedBook.Title) &&
                !string.IsNullOrEmpty(_bookListViewModel.SelectedBook.Author) &&
                !string.IsNullOrEmpty(_bookListViewModel.SelectedBook.ISBN) &&

                base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            Book book = new Book(_bookListViewModel.SelectedBook.ID, _bookListViewModel.SelectedBook.Title,
                _bookListViewModel.SelectedBook.Author, _bookListViewModel.SelectedBook.Description, _bookListViewModel.SelectedBook.ISBN);

            try
            {
                _library.EditBook(book);
                MessageBox.Show("Successfully Edited The Book.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _bookListViewNavigationService.Navigate(); //navigate to go to new book page
            }
            catch(BookEditOutForLoanException)
            {
                MessageBox.Show("Book cannot be as it is out for loan!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _bookListViewNavigationService.Navigate();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BookListViewModel.SelectedBook) ||
                e.PropertyName == nameof(BookListViewModel.SelectedBook.Title) ||
                e.PropertyName == nameof(BookListViewModel.SelectedBook.Author) ||
                e.PropertyName == nameof(BookListViewModel.SelectedBook.ISBN)) //||
                                                                               //e.PropertyName == nameof(BookListViewModel.SelectedBook.Description))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
