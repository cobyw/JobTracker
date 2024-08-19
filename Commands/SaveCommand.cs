using JobTracker.Data;
using JobTracker.Utilities;
using JobTracker.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobTracker.Commands
{
    public class SaveCommand : CommandBase
    {
        private readonly MainViewModel _mainViewModel;

        public SaveCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return JobManager.GetJobs().Count > 0;
        }

        public override void Execute(object parameter)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Job>));

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "XML Files(*.xml)|*.xml|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                Tools.WriteToXmlFile<ObservableCollection<Job>>(dialog.FileName, JobManager.GetJobs());
            }
            _mainViewModel.LastSaveTime = DateTime.Now;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecutedChanged();
        }
    }
}
