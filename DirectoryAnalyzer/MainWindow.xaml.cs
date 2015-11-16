using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DesktopAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            /*
             * <!--<mui:Link DisplayName="Help" Source="/View/Help.xaml" />
                <mui:Link DisplayName="Results" Source="/View/ResultsView.xaml" />-->
             * */
           
        }

        public void AddLink(string displayName, string linkUri)
        {
            MenuLinkGroups.First().Links.Add(new FirstFloor.ModernUI.Presentation.Link()
            {
                DisplayName = displayName,
                Source = new Uri(linkUri, UriKind.Relative)
            });
        }


        public void NavigateToLink(string displayName)
        {
            try
            {
                ContentSource = MenuLinkGroups.First().Links.Where(p => p.DisplayName == displayName).First().Source;
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show(String.Format("Error navigating to page {0}. Exception: {1}", displayName, ex.Message));
            }
        }

   
    }
}
