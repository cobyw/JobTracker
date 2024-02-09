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

namespace JobTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentSelectionIndex = 0;
        private List<JobInfo> jobs;

        public MainWindow()
        {
            InitializeComponent();

            jobList.DisplayMemberPath = "compoundTitle";

            jobList.Items.Add(new JobInfo());
            jobList.SelectedIndex = 0;

            LoadJobData(0);
            jobs = new List<JobInfo>();
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

                dateLocated.DisplayDate = ((JobInfo)jobList.Items[index]).dateLocated;
                dateMaterialsFinished.DisplayDate = ((JobInfo)jobList.Items[index]).dateMaterialsFinished;
                dateApplied.DisplayDate = ((JobInfo)jobList.Items[index]).dateApplied;
                dateNextSteps.DisplayDate = ((JobInfo)jobList.Items[index]).dateNextSteps;

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

            if (compoundTitle != null)
            {
                jobInfo.compoundTitle = compoundTitle.Content.ToString();
            }
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

            jobInfo.dateLocated = dateLocated.SelectedDate ?? DateTime.Now;
            jobInfo.dateMaterialsFinished = dateMaterialsFinished.SelectedDate ?? DateTime.Now;
            jobInfo.dateApplied = dateApplied.SelectedDate ?? DateTime.Now;
            jobInfo.dateNextSteps = dateNextSteps.SelectedDate ?? DateTime.Now;

            jobInfo.contactInfo = contactInfo.Text;
            jobInfo.notes = notes.Text;

            jobList.Items[currentSelectionIndex] = jobInfo;
        }

        private void compound_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCompoundTitle();
        }

        private void UpdateCompoundTitle()
        {
            if (companyName != null && jobTitle != null)
            {
                string currentTitle = string.Format("{0} - {1}", companyName.Text, jobTitle.Text);
                compoundTitle.Content = currentTitle;
            }
        }

        private void BtnAddJob_Click(object sender, RoutedEventArgs e)
        {

            jobList.Items.Add(new JobInfo());
            jobList.SelectedIndex = jobList.Items.Count - 1;
        }

        private void jobList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentSelectionIndex != jobList.SelectedIndex && jobList.SelectedIndex != -1)
            {
                StoreJobData();
                currentSelectionIndex = jobList.SelectedIndex;
                LoadJobData(jobList.SelectedIndex);
            }
        }

        private void compound_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            StoreJobData();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO - Add pop up if they haven't saved recently
        }


        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
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
                WriteToXmlFile<List<JobInfo>>(dialog.FileName, jobs);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                currentSelectionIndex = 0;
                jobs.Clear();
                jobs = ReadFromXmlFile<List<JobInfo>>(openFileDialog.FileName);
                jobList.Items.Clear();

                foreach (JobInfo info in jobs)
                {
                    jobList.Items.Add(info);
                }

                LoadJobData(currentSelectionIndex);
            }
        }

    }
}