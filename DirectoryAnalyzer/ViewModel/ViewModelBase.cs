using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAnalyzer.ViewModel
{
    /// <summary>
    /// An event for closing the current View and ViewModel
    /// </summary>
    public delegate void CloseEvent();


    /// <summary>
    /// Defines the common base class for all ViewModels
    /// </summary>
    public class ViewModelBase : BindableBase
    {
        public event CloseEvent Closed;

        /// <summary>
        /// Called when closing a viewmodel
        /// </summary>
        protected void Close()
        {
            if(Closed != null)
            {
                Closed();
            }
        }
    }
}
