using LibraryManagementApplication.Model;
using LibraryManagementApplication.Stores;
using LibraryManagementApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Services
{
    public class NavigationService
    {

        private readonly NavigationStore _navigationStore;
        private Func<ViewModelBase> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> createViewModel) //function that returns a view model base
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
