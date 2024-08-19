using JobTracker.Data;
using JobTracker.Utilities;
using JobTracker.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace JobTracker.Commands
{
    public class RemoveJobCommand : CommandBase
    {
        private readonly MainViewModel _mainViewModel;

        public RemoveJobCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return JobManager.GetJobs().Count > 1;
        }

        public override void Execute(object parameter)
        {
            if (JobManager.GetJobs().Count == 1)
            {
                MessageBox.Show("You can't remove your last job");
            }
            else if (MessageBox.Show("Are you sure you want to remove this job?", "Remove Job?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var initialSelectedIndex = _mainViewModel.CurrentSelectionIndex;
                if (initialSelectedIndex == JobManager.GetJobs().Count - 1)
                {
                    _mainViewModel.CurrentSelectionIndex--;
                }

                JobManager.RemoveJobAtIndex(initialSelectedIndex);
                _mainViewModel.Refresh();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecutedChanged();
        }
    }
}
