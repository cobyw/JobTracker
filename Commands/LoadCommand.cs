using JobTracker.Model;
using JobTracker.Utilities;
using JobTracker.ViewModel;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace JobTracker.Commands
{
    public class LoadCommand : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Dictionary<string, string> _replacementDictionary;

        public LoadCommand(MainViewModel mainViewModel, Dictionary<string, string> replacementDictionary)
        {
            _mainViewModel = mainViewModel;
            _replacementDictionary = replacementDictionary;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //do we need to replace anything
                var saveFileUpdated = false;
                if (_replacementDictionary?.Count > 0)
                {
                    saveFileUpdated = Tools.TryUpdateSaveFile(openFileDialog.FileName, _replacementDictionary);
                }


                //make sure we actually are loading a valid file
                var loadedJobs = Tools.ReadFromXmlFile<ObservableCollection<Job>>(openFileDialog.FileName);
                if (loadedJobs?.Count > 0)
                {
                    //clear our old jobs and re-add them so we don't break the data binding
                    JobManager.ClearJobs();
                    foreach (var job in loadedJobs)
                    {
                        JobManager.AddJob(job);
                    }
                    _mainViewModel.CurrentSelectionIndex = 0;
                    _mainViewModel.RefreshJobs();

                    if (saveFileUpdated)
                    {
                        MessageBox.Show("Save file successfully updated to current format and loaded.\nThe original file has been backed up in the same location.");
                    }

                    _mainViewModel.LastSaveTime = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Error - Unable to load file");
                }
            }
        }
    }
}
