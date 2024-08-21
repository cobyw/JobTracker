using JobTracker.Model;
using JobTracker.ViewModel;

namespace JobTracker.Commands
{
    /// <summary>
    /// Adds a new default job, refreshes the Job database, and selects the new job.
    /// </summary>
    public class AddJobCommand : CommandBase
    {
        private readonly MainViewModel _mainViewModel;

        public AddJobCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object parameter)
        {
            JobManager.AddNewJob();
            _mainViewModel.RefreshJobs();
            _mainViewModel.CurrentSelectionIndex = JobManager.JobCount() - 1;
        }
    }
}
