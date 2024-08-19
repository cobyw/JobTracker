using JobTracker.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static JobTracker.Model.ChangeData;

namespace JobTracker.ViewModel
{
    public class DateViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        public DateViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }


        /// <summary>
        /// Used to check what was changed vs the date the change occurred
        /// </summary>
        private List<ChangeData> _changeDatas = new List<ChangeData>();
        /// <summary>
        /// Used to fill in the calendar with the highlighted dates
        /// </summary>
        private HashSet<DateTime> _datesOfInterest = new HashSet<DateTime>();

        public HashSet<DateTime> DatesOfInterest
        {
            get
            {
                return GetDatesOfInterest();
            }
        }
        public string UpdatesOnDate
        {
            get
            {
                return GetUpdatesOnDate();
            }
        }


        /// <summary>
        /// Updates the change data of the list of jobs,
        /// Updates the dates of interest for those jobs
        /// Returns those interesting dates.
        /// This is used to populate the highlighted dates on the calendar.
        /// </summary>
        /// <param name="jobInfos">The job infos to be checked for change</param>
        /// <returns>The dates that any changes occurred.</returns>
        private HashSet<DateTime> GetDatesOfInterest()
        {
            UpdateChangeData();
            UpdateDatesOfInterest();

            return _datesOfInterest;
        }

        /// <summary>
        /// Displays the activities that happened on the specified day.
        /// Includes the day being queeried.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string GetUpdatesOnDate()
        {
            var retVal = String.Format("{0}:", _mainViewModel.SelectedDate?.ToShortDateString());
            bool hasInfo = false;

            foreach (ChangeData changeData in _changeDatas)
            {
                if (changeData.ChangeDate.Date == _mainViewModel.SelectedDate?.Date)
                {
                    retVal += string.Format("\n{0} - {1}", changeData.ChangeJob,
                        changeData.ChangeTypeString);
                    hasInfo = true;
                }
            }

            if (!hasInfo)
            {
                retVal += "\nNothing happened on this day.";
            }

            return retVal;
        }

        /// <summary>
        /// Gathers up and stores all the different types of changes in the jobInfos
        /// </summary>
        /// <param name="jobInfos"></param>
        private void UpdateChangeData()
        {
            _changeDatas.Clear();

            foreach (Job jobInfo in JobManager.GetJobs())
            {
                if (jobInfo.dateLocated != null)
                {
                    _changeDatas.Add(new ChangeData(jobInfo.dateLocated ?? DateTime.MinValue,
                        ChangeType.Located, jobInfo.jobTitle));
                }
                if (jobInfo.dateMaterialsFinished != null)
                {
                    _changeDatas.Add(new ChangeData(jobInfo.dateMaterialsFinished ?? DateTime.MinValue,
                    ChangeType.MaterialsFinished, jobInfo.jobTitle));
                }
                if (jobInfo.dateApplied != null)
                {
                    _changeDatas.Add(new ChangeData(jobInfo.dateApplied ?? DateTime.MinValue,
                    ChangeType.Applied, jobInfo.jobTitle));
                }
                if (jobInfo.dateNextSteps != null)
                {
                    _changeDatas.Add(new ChangeData(jobInfo.dateNextSteps ?? DateTime.MinValue,
                    ChangeType.NextSteps, jobInfo.jobTitle));
                }
            }
        }

        /// <summary>
        /// Clears out the currently stored dates of interest and repopulates
        /// it based on the stored change data
        /// </summary>
        private void UpdateDatesOfInterest()
        {
            _datesOfInterest.Clear();

            foreach (ChangeData changeData in _changeDatas)
            {
                if (!_datesOfInterest.Contains(changeData.ChangeDate))
                {
                    _datesOfInterest.Add(changeData.ChangeDate.Date);
                }
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.SelectedDate) ||
                e.PropertyName == nameof(MainViewModel.SelectedJob))
            {
                OnPropertyChanged(nameof(UpdatesOnDate));
            }
        }
    }
}
