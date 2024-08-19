using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using JobTracker.Commands;
using JobTracker.Data;
using JobTracker.Utilities;
using Microsoft.Win32;
using System.Diagnostics;

namespace JobTracker.ViewModel
{
	public class MainViewModel : ViewModelBase
	{

        private ObservableCollection<Job> _jobDatabase;

        public IEnumerable<Job> Jobs { get; set; }

        private Job _selectedJob;
        public Job SelectedJob
        {
            get { return _selectedJob; }
            set{ _selectedJob = value; }
        }

        private Date _selectedDate;
        public Date SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
            }
        }

        public string CurrentDateInfo { get; set; }

        private SelectedDatesCollection _selectedDates;

        //commands
        public ICommand	SaveCommand { get; }
		public ICommand	LoadCommand { get;}
		public ICommand	AddJobCommand { get; }
		public ICommand	RemoveJobCommand { get; }

		public MainViewModel()
		{
            _jobDatabase = JobManager.GetJobs();
            Jobs = _jobDatabase;

            SaveCommand = new SaveCommand(this);
            LoadCommand = new LoadCommand(this);
            AddJobCommand = new AddJobCommand(this);
			RemoveJobCommand = new RemoveJobCommand(this);
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

        Data.Date dateInfo = new Data.Date();
        DateTime? currentSelectedDate = DateTime.Now;


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

        /// Called when the user updates the job name or company name fields
        /// Updates the title at the top of the screen as they type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobCompany_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCompoundTitle();
        }

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

        /// <summary>
        /// Called when the calender is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCalendar();
        }

        /// <summary>
        /// Called whenever the user selects a date they have done something with the current job
        /// Ensures the calendar is up to date with their new selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCalendar();
        }

        #region Helper Methods
        /// <summary>
        /// Updates the calendar to mark which dates have information.
        /// We are using blackout dates instead of selections because they are
        /// still selectable and remain when the user selects things.
        /// </summary>
        private void UpdateCalendar()
        {

            foreach (DateTime date in dateInfo.GetDatesOfInterest(_jobDatabase))
            {
                _selectedDates.Add(date);
            }

            UpdateCalendarInfo(currentSelectedDate);
        }

        /// <summary>
        /// Updates the Calendar information box with the information for the selected day
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateCalendarInfo(DateTime? dateTime)
        {
            CurrentDateInfo = dateInfo.GetUpdatesOnDate(dateTime);
        }

        /// <summary>
        /// Updates the title at the top of the screen to
        /// reflect the current status, company title, and job title
        /// Also refreshes UI to ensure the title in the List is correct
        /// </summary>
        private void UpdateCompoundTitle()
        {
            //make sure count is greater than 0 so there aren't issues when loading in files
            if (_jobDatabase.Count > 0)
            {
                _jobDatabase[_currentSelectionIndex].UpdateCompoundTitle();
            }
        }
        #endregion
    }
}
