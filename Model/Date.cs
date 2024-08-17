using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobTracker.Data.Date.ChangeData;

namespace JobTracker.Data
{
    public class Date
    {
        /// <summary>
        /// ChangeData includes all of the information about the type of change that occurred in a job.
        /// For example, when a job was applied to.
        /// </summary>
        public struct ChangeData
        {
            /// <summary>
            /// The date of the change that was made to a job
            /// </summary>
            public DateTime ChangeDate
            {
                get;
            }
            /// <summary>
            /// The type of change that was made to a job
            /// </summary>
            public ChangeType TypeOfChange
            {
                get;
            }
            /// <summary>
            /// The name of the job that was changed
            /// </summary>
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

            /// <summary>
            /// The pontial types of changes that could have happened to a job
            /// </summary>
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
        /// A nicely formated dictionary of the types of changes
        /// </summary>
        private static Dictionary<ChangeType, string> changeTypeDictionary = new Dictionary<ChangeType, string>()
            {
            {ChangeType.Located, "Located" },
            {ChangeType.MaterialsFinished, "Materials Finished" },
            {ChangeType.Applied, "Applied" },
            {ChangeType.NextSteps, "Next Steps" },
            {ChangeType.Empty, "" },
            };


        /// <summary>
        /// Used to check what was changed vs the date the change occurred
        /// </summary>
        private List<ChangeData> changeDatas = new List<ChangeData>();
        /// <summary>
        /// Used to fill in the calendar with the highlighted dates
        /// </summary>
        private List<DateTime> dateOfInterest = new List<DateTime>();

        /// <summary>
        /// Returns a politely formated version of the change type
        /// </summary>
        /// <param name="changeType">The type of change to be translated</param>
        /// <returns>The nicely formated change</returns>
        private static string GetChangeTypeString(ChangeType changeType)
        {
            return changeTypeDictionary[changeType];
        }

        /// <summary>
        /// Updates the change data of the list of jobs,
        /// Updates the dates of interest for those jobs
        /// Returns those interesting dates.
        /// This is used to populate the highlighted dates on the calendar.
        /// </summary>
        /// <param name="jobInfos">The job infos to be checked for change</param>
        /// <returns>The dates that any changes occurred.</returns>
        public List<DateTime> GetDatesOfInterest(ObservableCollection<Job> jobInfos)
        {
            UpdateChangeData(jobInfos);
            UpdateDatesOfInterest();

            return dateOfInterest;
        }

        /// <summary>
        /// Gathers up and stores all the different types of changes in the jobInfos
        /// </summary>
        /// <param name="jobInfos"></param>
        private void UpdateChangeData(ObservableCollection<Job> jobInfos)
        {
            changeDatas = new List<ChangeData>();

            foreach (Job jobInfo in jobInfos)
            {
                if (jobInfo.dateLocated != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateLocated ?? DateTime.MinValue,
                        ChangeType.Located, Job.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateMaterialsFinished != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateMaterialsFinished ?? DateTime.MinValue,
                    ChangeType.MaterialsFinished, Job.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateApplied != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateApplied ?? DateTime.MinValue,
                    ChangeType.Applied, Job.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateNextSteps != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateNextSteps ?? DateTime.MinValue,
                    ChangeType.NextSteps, Job.GetCompoundTitle(jobInfo, includeStatus: false)));
                }
            }
        }

        /// <summary>
        /// Clears out the currently stored dates of interest and repopulates
        /// it based on the stored change data
        /// </summary>
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
