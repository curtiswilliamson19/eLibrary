using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged //this is the view that WPF automatically hooks into, view model can raise an event on this interface
    {
        public event PropertyChangedEventHandler PropertyChanged; //when this event raised, tell UI what bindings to update

        /// <summary>
        /// Call this method to tell UI whenever a property value has changed so that the views can re-grab the property value
        /// and update the UI
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
