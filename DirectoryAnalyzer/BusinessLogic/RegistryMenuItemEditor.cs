using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopAnalyzer.BusinessLogic
{
    /// <summary>
    /// Registry editing code is based on this CodeProject article: 
    /// http://www.codeproject.com/Articles/10104/Add%ADa%ADcontext%ADmenu%ADto%ADthe%ADWindows%ADExplorer2/6
    /// </summary>
    class RegistryMenuItemEditor : BindableBase
    {

        private string m_command;

        private string m_regEntryName;
        private string m_regCommandPath;


        private string m_menuItemText;
        public string MenuItemText
        {
            get { return m_menuItemText; }
            private set { SetProperty(ref m_menuItemText, value); }
        }

        public bool EntryExists { get; private set; }

        public RegistryMenuItemEditor(string entryName, string command)
        {
            m_command = command;
            m_regEntryName = @"Software\Classes\directory\shell\" + entryName;
         
            m_regCommandPath = m_regEntryName + "\\command";

            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            try
            {
                this.CheckSecurity();
                regmenu = Registry.CurrentUser.OpenSubKey(m_regEntryName, false);
                if (regmenu != null)
                {
                    MenuItemText = (String)regmenu.GetValue("");
                    EntryExists = true;
                }
                else
                {
                    EntryExists = false;
                }
            }

            catch (ArgumentException ex)
            {
                // RegistryPermissionAccess.AllAccess can not be used as a parameter for GetPathList.
                MessageBox.Show("An ArgumentException occured as a result of using AllAccess.  "
                  + "AllAccess cannot be used as a parameter in GetPathList because it represents more than one "
                  + "type of registry variable access : \n" + ex);
            }
            catch (SecurityException ex)
            {
                // RegistryPermissionAccess.AllAccess can not be used as a parameter for GetPathList.
                MessageBox.Show("An ArgumentException occured as a result of using AllAccess.  " + ex);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (regmenu != null)
                    regmenu.Close();
                if (regcmd != null)
                    regcmd.Close();
            }

        }

        public void UpdateEntry(string menuItemText)
        {  
            RegistryKey regmenu = null;
            RegistryKey baseKey = null;

            try
            {
                baseKey = Registry.CurrentUser; ;

                regmenu = baseKey.CreateSubKey(m_regEntryName);
                if (regmenu != null)
                    regmenu.SetValue("", menuItemText);
 
                MenuItemText = menuItemText;

                MessageBox.Show("Context menu text updated to: \"" + menuItemText +"\"");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (regmenu != null)
                    regmenu.Close();
            }

        }

        public void AddEntry(string menuItemText)
        {
            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            RegistryKey baseKey = null;
            try
            {
                baseKey = Registry.CurrentUser; ;
                regmenu = baseKey.CreateSubKey(m_regEntryName);
                if (regmenu != null)
                    regmenu.SetValue("", menuItemText);
                regcmd = baseKey.CreateSubKey(m_regCommandPath);
                if (regcmd != null)
                    regcmd.SetValue("", m_command);

                MenuItemText = menuItemText;
                EntryExists = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (regmenu != null)
                    regmenu.Close();
                if (regcmd != null)
                    regcmd.Close();
            }
        }

        public void RemoveEntry()
        {
            RegistryKey baseKey = null;

            try
            {
                baseKey = Registry.CurrentUser;
                RegistryKey reg = baseKey.OpenSubKey(m_regCommandPath);
                if (reg != null)
                {
                    reg.Close();
                    baseKey.DeleteSubKey(m_regCommandPath);
                }
                reg = baseKey.OpenSubKey(m_regEntryName);
                if (reg != null)
                {
                    reg.Close();
                    baseKey.DeleteSubKey(m_regEntryName);
                }

                EntryExists = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }




        private void CheckSecurity()
        {

            //check registry permissions
            RegistryPermission regPerm;
            //regPerm = new RegistryPermission(RegistryPermissionAccess.Write, "HKEY_CLASSES_ROOT\\" + m_regEntryName);
            //regPerm.AddPathList(RegistryPermissionAccess.Write, "HKEY_CLASSES_ROOT\\" + m_regCommandPath);

            regPerm = new RegistryPermission(RegistryPermissionAccess.Write, "HKEY_CURRENT_USER\\" + m_regEntryName);
            regPerm.AddPathList(RegistryPermissionAccess.Write, "HKEY_CURRENT_USER\\" + m_regCommandPath);

            regPerm.Demand();

        }

    }
}

