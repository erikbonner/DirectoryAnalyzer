using DesktopAnalyzer.BusinessLogic;
using DesktopAnalyzer.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopAnalyzer.ViewModel
{
    public class AnalyzerVm : ViewModelBase
    {
        // backing fields...
        private int m_progress;
        private string m_viewTitle;
        private ICommand m_analyzeCommand;

        private CancellationTokenSource m_cancellationToken = new CancellationTokenSource();

        /// <summary>
        /// The path that we will be analyzing
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Title of the view
        /// </summary>
        public string ViewTitle
        {
            get { return m_viewTitle; }
            set { SetProperty(ref m_viewTitle, value); }
        }
        
        /// <summary>
        /// Invokes the analysis
        /// </summary>
        public ICommand AnalyzeCommand
        {
            get
            {
                if (m_analyzeCommand == null)
                {
                    m_analyzeCommand = new DelegateCommand(DoAnalyze);
                }
                return m_analyzeCommand;
            }
        }


        /// <summary>
        /// The progress of the current analysis
        /// </summary>
        public int ProgressVal
        {
            get { return m_progress; }
            set { SetProperty(ref m_progress, value); }
        }

        private async void DoAnalyze()
        {
            Debug.Assert(TargetPath != null);

            // set the view title
            ViewTitle = "Analyzing " + TargetPath + "...";

            try
            {
                   // start the analyzer
                DirectoryAnalyzer analyzer = new DirectoryAnalyzer();
                var results = await Task.Run(() => analyzer.Analyze(TargetPath, new Progress<int>(ReportProgress), m_cancellationToken.Token));

                // at this point, we close ourselves and open the resuls view
                var resultsVm = new ResultsVm(results, TargetPath + " results");

                // add the results Vm to our application resources in order to pass it to the 
                // ResultsView.
                // yes, I know, this is terrible design. But for the time being, I 
                // don't have time to learn ModernUI well enough to understand how to do this 
                // correctly. 
                Application.Current.Resources.Add("ResultsVm", resultsVm);

                Close();
            }
            catch (OperationCanceledException)
            {
                Application.Current.Shutdown();
            }
         
        }

        public void Cancel()
        {
            m_cancellationToken.Cancel();
        }

        private void ReportProgress(int val)
        {
            ProgressVal = val;
        }
    }
}
