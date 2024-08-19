using JobTracker.Model;
using JobTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Commands
{
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
            _mainViewModel.Refresh();
            _mainViewModel.CurrentSelectionIndex = JobManager.JobCount() - 1;
        }
    }
}
