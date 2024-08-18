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
        public ICommand	SaveCommand { get; set; }
		public ICommand	LoadCommand { get; set; }
		public ICommand	AddJobCommand { get; set; }
		public ICommand	RemoveJobCommand { get; set; }

		public MainViewModel()
		{
            _jobDatabase = JobManager.GetJobs();
            Jobs = _jobDatabase;

            SaveCommand = new RelayCommand(Save, CanSave);
			LoadCommand = new RelayCommand(Load, CanLoad);
			AddJobCommand = new RelayCommand(AddJob, CanAddJob);
			RemoveJobCommand = new RelayCommand(RemoveJob, CanRemoveJob);
		}
        private void Save(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Job>));

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "XML Files(*.xml)|*.xml|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                Tools.WriteToXmlFile<ObservableCollection<Job>>(dialog.FileName, _jobDatabase);
                lastSaveTime = DateTime.Now;
            }
        }

        private bool CanSave(object obj)
        {
            return _jobDatabase.Count > 0;
        }
       
        private void Load(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //make sure we actually are loading a valid file
                var loadedJobs = Tools.ReadFromXmlFile<ObservableCollection<Job>>(openFileDialog.FileName);
                if (loadedJobs.Count > 0)
                {
                    //clear our old jobs and re-add them so we don't break the data binding
                    _jobDatabase.Clear();
                    foreach (var job in loadedJobs)
                    {
                        _jobDatabase.Add(job);
                    }
                    currentSelectionIndex = 0;
                }
                else
                {
                    MessageBox.Show("Error - Unable to load file");
                }
            }
        }

        private bool CanLoad(object obj)
        {
            return true;
        }

        private void AddJob(object obj)
        {
            _jobDatabase.Add(new Job());
            currentSelectionIndex = _jobDatabase.Count - 1;
        }
       
        private bool CanAddJob(object obj)
        {
            return true;
        }

        private void RemoveJob(object obj)
        {
            if (_jobDatabase.Count == 1)
            {
                MessageBox.Show("You can't remove your last job");
            }
            else if (MessageBox.Show("Are you sure you want to remove this job?", "Remove Job?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var initialSelectedIndex = currentSelectionIndex;
                if (initialSelectedIndex == _jobDatabase.Count - 1)
                {
                    currentSelectionIndex--;
                }

                _jobDatabase.RemoveAt(initialSelectedIndex);
            }
        }

        private bool CanRemoveJob(object obj)
        {
            return _jobDatabase.Count > 1;
        }

        //old stuff
        private int currentSelectionIndex = 0;

        Data.Date dateInfo = new Data.Date();
        DateTime? currentSelectedDate = DateTime.Now;

        private DateTime lastSaveTime = DateTime.MinValue;
        private const float c_TIMEBETWEENSAVEREMINDERS = 15f;



        #region XAML Events

        /*
        /// <summary>
        /// Called when the user goes into the Job or Company boxes.
        /// Clears out the text box if it has the default string, making it easier to type the desires string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobCompany_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender == jobTitle && jobTitle.Text == Job.c_JOBTITLE)
            {
                jobTitle.Text = string.Empty;
            }
            else if (sender == companyName && companyName.Text == Job.c_COMPANY)
            {
                companyName.Text = string.Empty;
            }
        }
        */


        /// <summary>
        /// Called when the user updates the job name or company name fields
        /// Updates the title at the top of the screen as they type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobCompany_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCompoundTitle();
        }

        /*
        /// <summary>
        /// Called when the user exits either the job or company boxes.
        /// If the box is blank it fills the default value back in
        /// It also stores the current information to make sure the name in the list box is accurate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobCompany_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender == jobTitle && jobTitle.Text == string.Empty)
            {
                jobTitle.Text = Job.c_JOBTITLE;
            }
            else if (sender == companyName && companyName.Text == string.Empty)
            {
                companyName.Text = Job.c_COMPANY;
            }

            UpdateCompoundTitle();
        }
        */

        /// <summary>
        /// Triggers when the user clicks the Add Job Button
        /// Adds a new job to the list and makes it the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddJob_Click(object sender, RoutedEventArgs e)
        {

            _jobDatabase.Add(new Job());
            currentSelectionIndex = _jobDatabase.Count - 1;
        }

        /// <summary>
        /// Triggers when the user clicks the Remove Job button.
        /// Removes the selected job from the list and selects the next job in the list.
        /// Ensures the user doesn't remove their last job
        /// and that they don't get an out of index exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemoveJob_Click(object sender, RoutedEventArgs e)
        {
            if (_jobDatabase.Count == 1)
            {
                MessageBox.Show("You can't remove your last job");
            }
            else if (MessageBox.Show("Are you sure you want to remove this job?", "Remove Job?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var initialSelectedIndex = currentSelectionIndex;
                if (initialSelectedIndex == _jobDatabase.Count - 1)
                {
                    currentSelectionIndex--;
                }

                _jobDatabase.RemoveAt(initialSelectedIndex);
            }
        }

        /// <summary>
        /// Encourages the user to save if they have not done so recently
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var timeSinceSave = DateTime.Now.Subtract(lastSaveTime).TotalSeconds;
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
        #endregion

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
                _jobDatabase[currentSelectionIndex].UpdateCompoundTitle();
            }
        }
        #endregion
    }
}
