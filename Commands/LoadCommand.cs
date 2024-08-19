using JobTracker.Data;
using JobTracker.Utilities;
using JobTracker.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JobTracker.Commands
{
    public class LoadCommand : CommandBase
    {
        private readonly MainViewModel _mainViewModel;

        public LoadCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //make sure we actually are loading a valid file
                var loadedJobs = Tools.ReadFromXmlFile<ObservableCollection<Job>>(openFileDialog.FileName);
                if (loadedJobs.Count > 0)
                {
                    //clear our old jobs and re-add them so we don't break the data binding
                    JobManager.ClearJobs();
                    foreach (var job in loadedJobs)
                    {
                        JobManager.AddJob(job);
                    }
                    _mainViewModel.CurrentSelectionIndex = 0;
                    _mainViewModel.Refresh();
                }
                else
                {
                    MessageBox.Show("Error - Unable to load file");
                }
            }
        }
    }
}
