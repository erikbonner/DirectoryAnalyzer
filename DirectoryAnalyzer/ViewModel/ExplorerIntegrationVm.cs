using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using DesktopAnalyzer.Properties;
using DesktopAnalyzer.BusinessLogic;

namespace DesktopAnalyzer.ViewModel
{
    class ExplorerIntegrationVm : ViewModelBase
    {
         private string m_contextItemText;
        public string ContextItemText
        {
            get { return m_contextItemText; }
            set { SetProperty(ref m_contextItemText, value); }
        }

        private DelegateCommand m_addCommand;
        public DelegateCommand AddCommand
        {
            get { return (m_addCommand ?? (m_addCommand = new DelegateCommand(OnAdd, CanAdd))) ; }
        }

        private DelegateCommand m_updateCommand;
        public DelegateCommand UpdateCommand
        {
            get { return (m_updateCommand ?? (m_updateCommand = new DelegateCommand(OnUpdate, CanUpdate))); }
        }

        private DelegateCommand m_removeCommand;
        public DelegateCommand RemoveCommand
        {
            get { return (m_removeCommand ?? (m_removeCommand = new DelegateCommand(OnRemove, CanRemove))); }
        }

        private RegistryMenuItemEditor m_registryMenuItem;


        public ExplorerIntegrationVm()
        {
            // the command that we will be savig in the registry will be the full path to 
            // our currently execution assembly + the name of the target program
            string currAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string command = "\"" + currAssemblyPath +  "\" \"%1\"";

            // create our RegistryMenuItemEditor instance to take care of registry operations
            m_registryMenuItem = new RegistryMenuItemEditor("DirectoryAnalyzer", command);
            if (m_registryMenuItem.EntryExists)
            {
                ContextItemText = m_registryMenuItem.MenuItemText;
            }
        }

        #region command callbacks

        private void OnAdd()
        {
            if(String.IsNullOrEmpty(ContextItemText))
            {
                MessageBox.Show("Please enter text!");
                return;
            }

            m_registryMenuItem.AddEntry(ContextItemText);
            UpdateCommands();

            //MessageBox.Show(
            //    String.Format("Sucessfully added {0} to window explorer context menu. \n\rContext menu item name: {1}", 
            //    Resources.ApplicationName, ContextItemText));
            
        }

        private bool CanAdd()
        {
            return !m_registryMenuItem.EntryExists;
        }

        private void OnRemove()
        {
            m_registryMenuItem.RemoveEntry();
            UpdateCommands();

            //MessageBox.Show(
            //   String.Format("Sucessfully removed {0} from window explorer context menu.",
            //   Resources.ApplicationName));
        }

        private bool CanRemove()
        {
            return m_registryMenuItem.EntryExists;
        }

        private void OnUpdate()
        {
            m_registryMenuItem.UpdateEntry(ContextItemText);

            //MessageBox.Show(
            //   String.Format("Sucessfully updated window explorer context menu. \n\rNew name: {0}",
            //   ContextItemText));
        }

        private bool CanUpdate()
        {
            return m_registryMenuItem.EntryExists && !String.IsNullOrEmpty(ContextItemText);
        }

        #endregion command callbacks

        private void UpdateCommands()
        {
            AddCommand.RaiseCanExecuteChanged();
            RemoveCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
        }
    }
}
