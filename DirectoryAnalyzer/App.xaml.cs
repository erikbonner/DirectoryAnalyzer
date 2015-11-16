using DesktopAnalyzer.View;
using DesktopAnalyzer.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

             // otherwise, we have more than one, which we don't support right now
            if (e.Args.Count() > 1)
            {
                MessageBox.Show("Program currently doesn't support more than one input at a time!");
                this.Dispatcher.InvokeShutdown();
                return;
            }

            var HelpPageName = DesktopAnalyzer.Properties.Resources.HelpPageName;
            var ExplorerIntegrationPageName = DesktopAnalyzer.Properties.Resources.ExplorerIntegrationPageName;
            var ResultsPageName = DesktopAnalyzer.Properties.Resources.ResutsPageName;

            // create our main window
            MainWindow mainWin = new MainWindow();
            mainWin.Title = DesktopAnalyzer.Properties.Resources.ApplicationName;
            mainWin.AddLink(HelpPageName, "/View/Help.xaml");
            mainWin.AddLink(ExplorerIntegrationPageName, "/View/ExplorerIntegrationView.xaml");

            // if we have no arguments, open the main window with the help screen
            if (e.Args.Count() == 0)
            {
                mainWin.NavigateToLink(HelpPageName);
                this.MainWindow = mainWin;
                this.MainWindow.Show();
            }

            // if we have one argument, directly start analyzing
            else if (e.Args.Count() == 1)
            {
                // add a results screen to the main window
                mainWin.AddLink(ResultsPageName, "View/ResultsView.xaml");

                // create our main window viewmodel
                var vm = new AnalyzerVm();
                vm.TargetPath = e.Args[0];
                vm.Closed += () =>
                {
                    mainWin.NavigateToLink(ResultsPageName);

                    this.MainWindow = mainWin;
                    this.MainWindow.Show();
                };

                var startWindow = new AnalyzerView(vm);
                this.MainWindow = startWindow;
                this.MainWindow.Show();

                // if we want to immediately start processing, invoke the processing
                if (vm.AnalyzeCommand.CanExecute(null))
                {
                    vm.AnalyzeCommand.Execute(null);
                }
            }

        }
    }
}
