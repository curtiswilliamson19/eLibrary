using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Commands
{
    public class ExitCommand : CommandBase
    {
        //Simply closes the application, binded to exit button
        public override void Execute(object parameter)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
