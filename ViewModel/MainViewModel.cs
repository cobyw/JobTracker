using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JobTracker.Commands;
using JobTracker.Data;

namespace JobTracker.ViewModel
{
	public class MainViewModel : ViewModelBase
	{

        private readonly ObservableCollection<Job> _jobDatabase;

        private readonly ObservableCollection<JobViewModel> _jobs;

        public IEnumerable<JobViewModel> Jobs => _jobs;

		//commands
		public ICommand	SaveCommand { get; set; }
		public ICommand	LoadCommand { get; set; }
		public ICommand	AddJobCommand { get; set; }
		public ICommand	RemoveJobCommand { get; set; }


		public MainViewModel()
		{
            _jobDatabase = JobManager.GetJobs();
            _jobs = new ObservableCollection<JobViewModel>();

			SaveCommand = new RelayCommand(Save, CanSave);
			LoadCommand = new RelayCommand(Load, CanLoad);
			AddJobCommand = new RelayCommand(AddJob, CanAddJob);
			RemoveJobCommand = new RelayCommand(RemoveJob, CanRemoveJob);
		}
        private void Save(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanSave(object obj)
        {
            throw new NotImplementedException();
        }
       
        private void Load(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddJob(object obj)
        {
            throw new NotImplementedException();
        }
        private bool CanLoad(object obj)
        {
            throw new NotImplementedException();
        }


        private bool CanAddJob(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveJob(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanRemoveJob(object obj)
        {
            throw new NotImplementedException();
        }


	}
}
