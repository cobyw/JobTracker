using JobTracker.Model;
using JobTracker.ViewModel;
using System.ComponentModel;
using System.Windows;

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
            return JobManager.JobCount() > 1;
        }

        public override void Execute(object parameter)
        {
            if (JobManager.JobCount() == 1)
            {
                MessageBox.Show("You can't remove your last job");
            }
            else if (MessageBox.Show("Are you sure you want to remove this job?", "Remove Job?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var initialSelectedIndex = _mainViewModel.CurrentSelectionIndex;

                var newSelectionIndex = initialSelectedIndex == JobManager.JobCount() - 1 ? initialSelectedIndex - 1 : initialSelectedIndex;

                JobManager.RemoveJobAtIndex(initialSelectedIndex);
                _mainViewModel.RefreshJobs();
                _mainViewModel.CurrentSelectionIndex = newSelectionIndex;
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecutedChanged();
        }
    }
}
