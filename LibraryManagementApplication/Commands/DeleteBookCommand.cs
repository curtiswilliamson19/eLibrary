using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services;
using LibraryManagementApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementApplication.Commands
{
    public class DeleteBookCommand : CommandBase
    {

        private readonly BookListViewModel _bookListViewModel;
        private readonly Library _library;
        private readonly NavigationService _bookListViewNavigationService;

        public DeleteBookCommand(BookListViewModel bookListViewModel, Library library, NavigationService bookListViewNavigationService)
        {
            _bookListViewModel = bookListViewModel;
            _library = library;
            _bookListViewNavigationService = bookListViewNavigationService;
            _bookListViewModel.PropertyChanged += OnViewModelPropertyChanged;
           
        }

        public override void Execute(object parameter)
        {
            if (_bookListViewModel.SelectedBook != null)
            {
                try
                {
                    _library.DeleteBookByID(_bookListViewModel.SelectedBook.ID);
                    _bookListViewModel.SelectedBook = null;
                    MessageBox.Show("Successfully Deleted The Book.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    _bookListViewNavigationService.Navigate(); //navigate to refresh page

                }
                catch (BookDeleteException)
                {
                    MessageBox.Show("There is no book with a matching ID to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BookDeleteOutForLoanException)
                {
                    MessageBox.Show("Sorry, this book is currently out for loan.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
   
        //If the selected book != null, then can execute
        public override bool CanExecute(object parameter)
        {
            return _bookListViewModel.SelectedBook != null;
        }

        //If book is selected, then can try to delete
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BookListViewModel.SelectedBook))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
