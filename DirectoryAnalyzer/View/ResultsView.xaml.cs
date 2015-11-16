using DesktopAnalyzer.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopAnalyzer.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ResultsView : UserControl
    {
        public ResultsView()
        {
            InitializeComponent();

            // fetch the pre-constructed view model from our application resources.
            // This is terrible design and should be done properly in future. 
            if(Application.Current.Resources.Contains("ResultsVm"))
            {
                this.DataContext = Application.Current.Resources["ResultsVm"];
            }

        }
    }
}
