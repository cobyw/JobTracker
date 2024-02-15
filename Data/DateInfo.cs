using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobTracker.Data.DateInfo.ChangeData;

namespace JobTracker.Data
{
    public class DateInfo
    {
        public struct ChangeData
        {
            public DateTime ChangeDate
            {
                get;
            }
            public ChangeType TypeOfChange
            {
                get;
            }
            public string ChangeJob
            {
                get;
            }

            public ChangeData(DateTime changeDate, ChangeType typeOfChange, string changeJob)
            {
                ChangeDate = changeDate;
                TypeOfChange = typeOfChange;
                ChangeJob = changeJob;
            }

            public enum ChangeType
            {
                Empty, //0
                Located, //1
                MaterialsFinished, //2
                Applied, //3
                NextSteps
            }
        }

        /// <summary>
        /// Used to check what was changed vs the date the change occurred
        /// </summary>
        private List<ChangeData> changeDatas = new List<ChangeData>();
        /// <summary>
        /// Used to fill in the calendar with the highlighted dates
        /// </summary>
        private List<DateTime> dateOfInterest = new List<DateTime>();

        private static string GetChangeTypeString(ChangeType changeType)
        {
            var changeTypeDictionary = new Dictionary<ChangeType, string>()
            {
            {ChangeType.Located, "Located" },
            {ChangeType.MaterialsFinished, "Materials Finished" },
            {ChangeType.Applied, "Applied" },
            {ChangeType.NextSteps, "Next Steps" },
            {ChangeType.Empty, "" },
            };

            return changeTypeDictionary[changeType];
        }

        public List<DateTime> GetDatesOfInterest(ObservableCollection<JobInfo> jobInfos)
        {
            UpdateChangeData(jobInfos);
            UpdateDatesOfInterest();

            return dateOfInterest;
        }

        private void UpdateChangeData(ObservableCollection<JobInfo> jobInfos)
        {
            changeDatas = new List<ChangeData>();

            foreach (JobInfo jobInfo in jobInfos)
            {
                if (jobInfo.dateLocated != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateLocated ?? DateTime.MinValue,
                        ChangeType.Located, JobInfo.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateMaterialsFinished != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateMaterialsFinished ?? DateTime.MinValue,
                    ChangeType.MaterialsFinished, JobInfo.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateApplied != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateApplied ?? DateTime.MinValue,
                    ChangeType.Applied, JobInfo.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateNextSteps != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateNextSteps ?? DateTime.MinValue,
                    ChangeType.NextSteps, JobInfo.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
            }
        }

        private void UpdateDatesOfInterest()
        {
            dateOfInterest = new List<DateTime>();

            foreach (ChangeData changeData in changeDatas)
            {
                if (!dateOfInterest.Contains(changeData.ChangeDate))
                {
                    dateOfInterest.Add(changeData.ChangeDate.Date);
                }
            }
        }

        /// <summary>
        /// Displays the activities that happened on the specified day.
        /// Includes the day being queeried.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string GetUpdatesOnDate(DateTime? dateTime)
        {
            var retVal = String.Format("{0}:", dateTime?.ToShortDateString());
            bool hasInfo = false;

            foreach (ChangeData changeData in changeDatas)
            {
                if (changeData.ChangeDate.Date == dateTime?.Date)
                {
                    retVal += String.Format("\n{0} - {1}", changeData.ChangeJob,
                        GetChangeTypeString(changeData.TypeOfChange));
                    hasInfo = true;
                }
            }

            if (!hasInfo)
            {
                retVal += "\nNothing happened on this day.";
            }

            return retVal;
        }
    }
}
