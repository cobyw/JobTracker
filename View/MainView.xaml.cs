using System.Windows;
using JobTracker.ViewModel;

namespace JobTracker.View
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            MainViewModel mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
        }
    }
}