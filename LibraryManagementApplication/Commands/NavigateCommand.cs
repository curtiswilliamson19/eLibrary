using LibraryManagementApplication.Services;
using LibraryManagementApplication.Stores;
using LibraryManagementApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Commands
{
    public class NavigateCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public NavigateCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
