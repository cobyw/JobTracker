﻿using JobTracker.Data;
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

namespace JobTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentSelectionIndex = 0;
        private List<JobInfo> jobs;
        DateInfo dateInfo;
        DateTime? currentSelectedDate;

        private DateTime lastSaveTime = DateTime.MinValue;
        private const float c_TIMEBETWEENSAVEREMINDERS = 15f;

        public MainWindow()
        {
            InitializeComponent();

            jobList.DisplayMemberPath = "compoundTitle";

            jobList.Items.Add(new JobInfo());
            jobList.SelectedIndex = 0;

            LoadJobData(0);
            jobs = new List<JobInfo>();
            dateInfo = new DateInfo();

            currentSelectedDate = DateTime.Now;

            UpdateCalendar();
            
        }

        /// <summary>
        /// Updates the calendar to mark which dates have information.
        /// We are using blackout dates instead of selections because they are
        /// still selectable and remain when the user selects things.
        /// </summary>
        private void UpdateCalendar()
        {
            UpdateJobList();

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
        /// Loads the data from the list into the form.
        /// </summary>
        public void LoadJobData(int index)
        {
            if (jobList.Items[index] != null && jobList.Items[index] is JobInfo)
            {
                jobTitle.Text = ((JobInfo)jobList.Items[index]).jobTitle;
                companyName.Text = ((JobInfo)jobList.Items[index]).companyName;
                URL.Text = ((JobInfo)jobList.Items[index]).URL;

                found.IsChecked = ((JobInfo)jobList.Items[index]).found;
                researched.IsChecked = ((JobInfo)jobList.Items[index]).researched;
                coverLetter.IsChecked = ((JobInfo)jobList.Items[index]).coverLetter;
                resume.IsChecked = ((JobInfo)jobList.Items[index]).resume;
                applied.IsChecked = ((JobInfo)jobList.Items[index]).applied;
                interviewing.IsChecked = ((JobInfo)jobList.Items[index]).interviewing;

                accepted.IsChecked = ((JobInfo)jobList.Items[index]).accepted;
                rejected.IsChecked = ((JobInfo)jobList.Items[index]).rejected;

                dateLocated.SelectedDate = ((JobInfo)jobList.Items[index]).dateLocated;
                dateMaterialsFinished.SelectedDate = ((JobInfo)jobList.Items[index]).dateMaterialsFinished;
                dateApplied.SelectedDate = ((JobInfo)jobList.Items[index]).dateApplied;
                dateNextSteps.SelectedDate = ((JobInfo)jobList.Items[index]).dateNextSteps;

                dateLocated.UpdateLayout();
                dateMaterialsFinished.UpdateLayout();
                dateApplied.UpdateLayout();
                dateNextSteps.UpdateLayout();

                compoundTitle.Content = ((JobInfo)jobList.Items[index]).compoundTitle;

                contactInfo.Text = ((JobInfo)jobList.Items[index]).contactInfo;
                notes.Text = ((JobInfo)jobList.Items[index]).notes;
            }
        }

        /// <summary>
        /// Stores the data that is in the form back into the list.
        /// </summary>
        public void StoreJobData()
        {
            JobInfo jobInfo = new JobInfo();

            jobInfo.jobTitle = jobTitle.Text;
            jobInfo.companyName = companyName.Text;
            jobInfo.URL = URL.Text;

            jobInfo.found = found.IsChecked ?? false;
            jobInfo.researched = researched.IsChecked ?? false;
            jobInfo.coverLetter = coverLetter.IsChecked ?? false;
            jobInfo.resume = resume.IsChecked ?? false;
            jobInfo.applied = applied.IsChecked ?? false;
            jobInfo.interviewing = interviewing.IsChecked ?? false;

            jobInfo.accepted = accepted.IsChecked ?? false;
            jobInfo.rejected = rejected.IsChecked ?? false;

            jobInfo.dateLocated = dateLocated.SelectedDate ?? null;
            jobInfo.dateMaterialsFinished = dateMaterialsFinished.SelectedDate ?? null;
            jobInfo.dateApplied = dateApplied.SelectedDate ?? null;
            jobInfo.dateNextSteps = dateNextSteps.SelectedDate ?? null;

            jobInfo.contactInfo = contactInfo.Text;
            jobInfo.notes = notes.Text;

            jobInfo.status = JobInfo.GetStatus(accepted: accepted.IsChecked ?? false,
                    rejected: rejected.IsChecked ?? false, interviewing: interviewing.IsChecked ?? false,
                    applied: applied.IsChecked ?? false, researched: researched.IsChecked ?? false,
                    coverLetter: coverLetter.IsChecked ?? false, resume: resume.IsChecked ?? false);
            jobInfo.compoundTitle = JobInfo.GetCompoundTitle(companyName: jobInfo.companyName, jobTitle: jobInfo.jobTitle, status:jobInfo.status);

            jobList.Items[currentSelectionIndex] = jobInfo;

            UpdateCalendar();
        }

        /// <summary>
        /// Updates the title at the top of the screen to
        /// reflect the current status, company title, and job title
        /// </summary>
        private void UpdateCompoundTitle()
        {
            if (companyName != null && jobTitle != null)
            {
                JobInfo.Status status = JobInfo.GetStatus(accepted: accepted.IsChecked ?? false,
                    rejected: rejected.IsChecked ?? false, interviewing: interviewing.IsChecked ?? false,
                    applied: applied.IsChecked ?? false, researched: researched.IsChecked ?? false,
                    coverLetter: coverLetter.IsChecked ?? false, resume: resume.IsChecked ?? false);

                compoundTitle.Content = JobInfo.GetCompoundTitle(companyName: companyName.Text, jobTitle: jobTitle.Text, status);
            }
        }


        /// <summary>
        /// Ensures the job list is up to date with the jobList ui.
        /// TODO: Make everything run on the job list and appropriately bind the jobList UI to the job list
        /// </summary>
        private void UpdateJobList()
        {
            jobs.Clear();

            foreach (JobInfo job in jobList.Items)
            {
                jobs.Add(job);
            }
        }

        /// <summary>
        /// Called when the user goes into the Job or Company boxes.
        /// Clears out the text box if it has the default string, making it easier to type the desires string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobCompany_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender == jobTitle && jobTitle.Text == JobInfo.c_JOBTITLE)
            {
                jobTitle.Text = string.Empty;
            }
            else if (sender == companyName && companyName.Text == JobInfo.c_COMPANY)
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
                jobTitle.Text = JobInfo.c_JOBTITLE;
            }
            else if (sender == companyName && companyName.Text == string.Empty)
            {
                companyName.Text = JobInfo.c_COMPANY;
            }

            StoreJobData();
        }

        /// <summary>
        /// Triggers when the user clicks the Add Job Button
        /// Adds a new job to the list and makes it the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddJob_Click(object sender, RoutedEventArgs e)
        {

            jobList.Items.Add(new JobInfo());
            jobList.SelectedIndex = jobList.Items.Count - 1;
        }

        /* TODO: Refactor required
        private void BtnRemoveJob_Click(object sender, RoutedEventArgs e)
        {
            if (jobList.Items.Count == 1)
            {
                MessageBox.Show("You can't remove your last job");
            }
            else if (MessageBox.Show("Are you sure you want to remove this job?", "Remove Job?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                jobList.Items.RemoveAt(currentSelectionIndex);

                if (currentSelectionIndex != 0)
                {
                    currentSelectionIndex--;
                }
            }

            UpdateJobList();
        }
        */

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
                StoreJobData();
                currentSelectionIndex = jobList.SelectedIndex;
                LoadJobData(jobList.SelectedIndex);
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StoreJobData();

            foreach (JobInfo info in jobList.Items)
            {
                jobs.Add(info);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<JobInfo>));

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "XML Files(*.xml)|*.xml|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                Tools.WriteToXmlFile<List<JobInfo>>(dialog.FileName, jobs);
                lastSaveTime = DateTime.Now;
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                currentSelectionIndex = 0;
                jobs.Clear();
                jobs = Tools.ReadFromXmlFile<List<JobInfo>>(openFileDialog.FileName);
                jobList.Items.Clear();

                foreach (JobInfo info in jobs)
                {
                    jobList.Items.Add(info);
                }

                LoadJobData(currentSelectionIndex);
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
            //calling this here because it forces the listbox to refresh the title
            StoreJobData();
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO: Refactor required 
        }
    }
}