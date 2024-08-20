using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using JobTracker.Commands;
using JobTracker.Model;
using System.ComponentModel;

namespace JobTracker.ViewModel
{
    public class MainViewModel : ViewModelBase
	{

        private ObservableCollection<JobViewModel> _jobDatabase = new ObservableCollection<JobViewModel>();

        public IEnumerable<JobViewModel> Jobs { get; set; }


        private JobViewModel _selectedJob;
        public JobViewModel SelectedJob
        {
            get
            {
                return _selectedJob;
            }
            set
            {
                _selectedJob = value;
                OnPropertyChanged(nameof(SelectedJob));
            }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private DateViewModel _dateViewModel;
        public DateViewModel DateViewModel
        {
            get
            {
                return  _dateViewModel;
            }
            set
            {
                 _dateViewModel = value;
                OnPropertyChanged(nameof(DateViewModel));
            }
        }

        //commands
        public ICommand	SaveCommand { get; }
		public ICommand	LoadCommand { get;}
		public ICommand	AddJobCommand { get; }
		public ICommand	RemoveJobCommand { get; }
		public ICommand	CloseCommand { get; }

		public MainViewModel()
		{
            if (JobManager.JobCount() == 0)
            {
                JobManager.AddNewJob();
            }

            RefreshJobs();

            _dateViewModel = new DateViewModel(this);
            SelectedDate = DateTime.Now;

            SaveCommand = new SaveCommand(this);
            LoadCommand = new LoadCommand(this);
            AddJobCommand = new AddJobCommand(this);
			RemoveJobCommand = new RemoveJobCommand(this);
            CloseCommand = new CloseCommand(this);
		}

        public void RefreshJobs()
        {
            foreach (JobViewModel job in _jobDatabase)
            {
                job.PropertyChanged -= OnJobViewModelPropertyChanged;
            }

            _jobDatabase.Clear();

            foreach (Job job in JobManager.GetJobs())
            {
                _jobDatabase.Add(new JobViewModel(job));

                //subscribe to the most recent member of the list
                _jobDatabase[^1].PropertyChanged += OnJobViewModelPropertyChanged;
            }

            Jobs = _jobDatabase;
        }

        //old stuff
        private int _currentSelectionIndex;
        public int CurrentSelectionIndex
        {
            get
            {
                return _currentSelectionIndex;
            }
            set
            {
                _currentSelectionIndex = value;
                OnPropertyChanged(nameof(CurrentSelectionIndex));
            }
        }


        public const double c_TIMEBETWEENSAVEREMINDERS = 15f;

        private DateTime _lastSaveTime = DateTime.MinValue;
        public DateTime LastSaveTime
        {
            get
            {
                return _lastSaveTime;
            }
            set
            {
                _lastSaveTime = value;
            }
        }

        private void OnJobViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(JobViewModel.DateLocated) ||
                e.PropertyName == nameof(JobViewModel.DateApplied) ||
                e.PropertyName == nameof(JobViewModel.DateMaterialsFinished) ||
                e.PropertyName == nameof(JobViewModel.DateNextSteps))
            {
                OnPropertyChanged(nameof(DateViewModel));
            }
        }

    }
}
