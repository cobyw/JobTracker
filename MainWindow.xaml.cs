using JobTracker.Data;
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

namespace JobTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<JobInfo> jobs;
        private int currentSelection = 0;

        public MainWindow()
        {
            InitializeComponent();

            jobList.ItemsSource = jobs;
            jobList.DisplayMemberPath = "jobTitle";

            jobs = new List<JobInfo>();

            jobList.Items.Add(new JobInfo("test"));
        }

        private void jobTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCompoundTitle();
        }

        private void companyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCompoundTitle();
        }

        private void UpdateCompoundTitle()
        {
            if (companyName != null && jobTitle != null)
            {
                compoundTitle.Content = string.Format("{0} - {1}", companyName.Text, jobTitle.Text);
            }
        }

        private void BtnAddJob_Click(object sender, RoutedEventArgs e)
        {

            jobList.Items.Add(new JobInfo("test"));
        }

        private void jobList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}