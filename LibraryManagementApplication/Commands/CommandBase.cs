using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagementApplication.Commands
{
    public abstract class CommandBase : ICommand
    {
        //UI listen for this event and hook into this interface
        public event EventHandler CanExecuteChanged;

        //method that tells if the command can execute
        //if this returns fails, button on view is disabled
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        //This will execute when button clicked that command is bound to
        //All that inherit CommandBase will have to implement
        public abstract void Execute(object parameter);

        //raises CanExecuteChangedEvent
        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
