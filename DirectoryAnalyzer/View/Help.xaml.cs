using FirstFloor.ModernUI.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Navigation;

namespace DesktopAnalyzer.View
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl
    {
        public Help()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when hyperlink (to Explorer Integration tool) is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWin = (MainWindow)Application.Current.MainWindow;
                mainWin.NavigateToLink(Properties.Resources.ExplorerIntegrationPageName);
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Error navigating to desired page.");
            }
        }
    }
}
