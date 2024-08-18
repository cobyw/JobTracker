using JobTracker.Data;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using JobTracker.Utilities;
using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.ObjectModel;
using System.Printing;

namespace JobTracker
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private int currentSelectionIndex = 0;
        public ObservableCollection<Job> jobs { get; set; } = new ObservableCollection<Job>() { new Job() };
        Data.Date dateInfo = new Data.Date();
        DateTime? currentSelectedDate = DateTime.Now;

        private DateTime lastSaveTime = DateTime.MinValue;
        private const float c_TIMEBETWEENSAVEREMINDERS = 15f;

        RoutedCommand saveCommand = new RoutedCommand();
        RoutedCommand newJobCommand = new RoutedCommand();

        #region XAML Events

        public MainView()
        {
            InitializeComponent();
            DataContext = this;

            jobList.SelectedIndex = 0;
            UpdateCalendar();

            saveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(saveCommand, BtnSave_Click));

            newJobCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newJobCommand, BtnAddJob_Click));
        }

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

            UpdateCompoundTitle() ;
        }

        /// <summary>
        /// Triggers when the user clicks the Add Job Button
        /// Adds a new job to the list and makes it the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddJob_Click(object sender, RoutedEventArgs e)
        {

            jobs.Add(new Job());
            jobList.SelectedIndex = jobList.Items.Count - 1;
            currentSelectionIndex = jobList.SelectedIndex;
            RefreshUI();
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
            if (jobs.Count == 1)
            {
                MessageBox.Show("You can't remove your last job");
            }
            else if (MessageBox.Show("Are you sure you want to remove this job?", "Remove Job?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var initialSelectedIndex = jobList.SelectedIndex;
                if (initialSelectedIndex == jobs.Count-1)
                {
                    currentSelectionIndex--;
                    jobList.SelectedIndex = 0;
                }

                jobs.RemoveAt(initialSelectedIndex);
                jobList.SelectedIndex = currentSelectionIndex;
            }
        }

        /// <summary>
        /// Triggers when the user selects a different job from the list
        /// Stores the current data to make sure it is persisted.
        /// Loads the selected data into the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jobList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentSelectionIndex != jobList.SelectedIndex && jobList.SelectedIndex != -1)
            {
                currentSelectionIndex = jobList.SelectedIndex;
                RefreshUI();
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
        /// Opens a save dialogue and saves all current data to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Job>));

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "XML Files(*.xml)|*.xml|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                Tools.WriteToXmlFile<ObservableCollection<Job>>(dialog.FileName, jobs);
                lastSaveTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Opens a load dialogue and replaces current data with the new data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //make sure we actually are loading a valid file
                var loadedJobs = Tools.ReadFromXmlFile<ObservableCollection<Job>>(openFileDialog.FileName);
                if (loadedJobs.Count > 0)
                {
                    //clear our old jobs and re-add them so we don't break the data binding
                    jobs.Clear();
                    foreach (var job in loadedJobs)
                    {
                        jobs.Add(job);
                    }
                    currentSelectionIndex = 0;
                    RefreshUI();
                }
                else
                {
                    MessageBox.Show("Error - Unable to load file");
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
            currentSelectedDate = calendar.SelectedDate;
            //update the calendar here to make the visibly selected dates
            //the dates that have information on them
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

            foreach (DateTime date in dateInfo.GetDatesOfInterest(jobs))
            {
                calendar.SelectedDates.Add(date);
            }

            UpdateCalendarInfo(currentSelectedDate);
        }

        /// <summary>
        /// Updates the Calendar information box with the information for the selected day
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateCalendarInfo(DateTime? dateTime)
        {
            calendarInfo.Text = dateInfo.GetUpdatesOnDate(dateTime);
        }

        /// <summary>
        /// Updates the title at the top of the screen to
        /// reflect the current status, company title, and job title
        /// Also refreshes UI to ensure the title in the List is correct
        /// </summary>
        private void UpdateCompoundTitle()
        {
            //make sure count is greater than 0 so there aren't issues when loading in files
            if (jobs.Count > 0)
            {
                jobs[currentSelectionIndex].UpdateCompoundTitle();
                RefreshUI();
            }
        }

        /// <summary>
        /// Refreshes the jobList UI as well as the title at the top of the job section
        /// </summary>
        private void RefreshUI()
        {
            jobList.Items.Refresh();

            //compound title needs to be manually refreshed to ensure it is redrawn appropriately
            compoundTitle.Content = jobs[currentSelectionIndex].compoundTitle;
        }
        #endregion
    }
}