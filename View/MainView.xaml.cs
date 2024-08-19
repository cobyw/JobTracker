using System.Windows;
using JobTracker.ViewModel;

namespace JobTracker.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            MainViewModel mainViewModel = new MainViewModel();
            DataContext = mainViewModel;

            /*
            jobList.SelectedIndex = 0;
            UpdateCalendar();

            saveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(saveCommand, BtnSave_Click));

            newJobCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newJobCommand, BtnAddJob_Click));
            */
        }
    }
}