using JobTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JobTracker.Commands
{
    public class CloseCommand : CommandBase
    {
        private readonly MainViewModel _mainViewModel;

        public CloseCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object parameter)
        {

            double timeSinceSave = DateTime.Now.Subtract(_mainViewModel.LastSaveTime).TotalSeconds;
            if (timeSinceSave > MainViewModel.c_TIMEBETWEENSAVEREMINDERS)
            {
                string warningMessage;

                if (timeSinceSave > 60)
                {
                    warningMessage = "You haven't saved in over a minute. Would you like to save?";
                }
                else
                {
                    warningMessage = string.Format("You haven't saved in {0} seconds. Would you like to save?", (int)timeSinceSave);
                }

                if (MessageBox.Show(string.Format(warningMessage, timeSinceSave), "Warning - You haven't saved", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var tryingToSave = true;
                    while (tryingToSave && timeSinceSave > MainViewModel.c_TIMEBETWEENSAVEREMINDERS)
                    {
                        _mainViewModel.SaveCommand.Execute(this);
                        if (timeSinceSave > MainViewModel.c_TIMEBETWEENSAVEREMINDERS)
                        {
                            if (MessageBox.Show("You failed to save, would you still like to save?", "Warning - You haven't saved", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            {
                                tryingToSave = false;
                            }
                        }
                    }
                }
            }

            
        }
    }
}
