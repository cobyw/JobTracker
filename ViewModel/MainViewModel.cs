using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using JobTracker.Commands;
using JobTracker.Model;

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
		}

        public void RefreshJobs()
        {
            _jobDatabase.Clear();

            foreach (Job job in JobManager.GetJobs())
            {
                _jobDatabase.Add(new JobViewModel(job));
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


        private const float c_TIMEBETWEENSAVEREMINDERS = 15f;

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

        /*
        /// <summary>
        /// Encourages the user to save if they have not done so recently
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var timeSinceSave = DateTime.Now.Subtract(_lastSaveTime).TotalSeconds;
            if (timeSinceSave > c_TIMEBETWEENSAVEREMINDERS)
            {
                string warningMessage;

                if (timeSinceSave > 60)
                {
                    warningMessage = "You haven't saved in over a minute. Are you sure you want to quit without saving?";
                }
                else
                {
                    warningMessage = string.Format("You haven't saved in {0} seconds. Are you sure you want to quit without saving?", (int)timeSinceSave);
                }

                if (MessageBox.Show(string.Format(warningMessage, timeSinceSave), "Warning - You haven't saved", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        */

        /// <summary>
        /// Called when the user begins to select a date
        /// Sets the default date to "now" so they see the current month
        /// in the calendar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((DatePicker)sender).SelectedDate == DateTime.MinValue || ((DatePicker)sender).SelectedDate == null)
            {
                ((DatePicker)sender).SelectedDate = DateTime.Now;
            }
        }

        /*
        /// <summary>
        /// Called whenever the user ticks or unticks any of the status checkboxes.
        /// Ensures the title is up to date with the current combined status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateCompoundTitle();
        }
        */

        #region Helper Methods
        
        #endregion
    }
}
