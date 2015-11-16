using DesktopAnalyzer.ViewModel;
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

namespace DesktopAnalyzer.View
{
    /// <summary>
    /// Interaction logic for AnalyzerView.xaml
    /// </summary>
    public partial class AnalyzerView : Window
    {
        public AnalyzerView(AnalyzerVm vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            vm.Closed += () => this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AnalyzerVm vm = this.DataContext as AnalyzerVm;
            if(vm != null)
            {
                vm.Cancel();
            }
        }
    }
}
