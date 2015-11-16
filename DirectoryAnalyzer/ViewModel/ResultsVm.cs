using DesktopAnalyzer.BusinessLogic;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopAnalyzer.ViewModel
{
    public class ResultsVm : ViewModelBase
    {

        // backing fields...
        private string m_selectedDisplayOption;
        private string m_sizeHeaderString;
        private string m_title;


        /// <summary>
        /// The currently selected option for MB/KB/B display option
        /// </summary>
        public string SelectedDisplayOption
        {
            get { return m_selectedDisplayOption; }
            set {
                if(SetProperty(ref m_selectedDisplayOption , value))
                {
                    SizeHeaderString = "Size (" + m_selectedDisplayOption + ")";
                    ProcessResults();
                }
            }
        }

        public string SizeHeaderString
        {
            get { return m_sizeHeaderString; }
            set { SetProperty(ref m_sizeHeaderString, value); }
        }

        /// <summary>
        /// Title of the view
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set { SetProperty(ref m_title, value); }
        }


        /// <summary>
        /// The possible display options
        /// </summary>
        public ObservableCollection<string> SizeDisplayOptions { get; set; }
            
        /// <summary>
        /// Stores the results of our output
        /// </summary>
        public ObservableCollection<DirectoryAnalyzer.DirectoryEntry> Results { get; set; }

        /// <summary>
        /// Raw results returned by our internal analyzer
        /// </summary>
        private IList<DirectoryAnalyzer.DirectoryEntry> m_rawResults;

        public ResultsVm(IList<DirectoryAnalyzer.DirectoryEntry> rawResults, string title)
        {
            m_rawResults = rawResults;
            Title = title;
            Results = new ObservableCollection<DirectoryAnalyzer.DirectoryEntry>();
            SizeDisplayOptions = new ObservableCollection<string>() { "MB", "KB", "B" };
            SelectedDisplayOption = "MB";
        }


        private void ProcessResults()
        {
            if (m_rawResults == null)
            {
                return;
            }

            // for now, just build up our output string
            Results.Clear();
            foreach (var dir in m_rawResults)
            {

                dir.Units = (DirectoryAnalyzer.DirectoryEntry.SizeUnits)
                    Enum.Parse(typeof(DirectoryAnalyzer.DirectoryEntry.SizeUnits), SelectedDisplayOption);
                Results.Add(dir);
            }
        }
    }
}
