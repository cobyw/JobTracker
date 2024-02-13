using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Markup;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using static JobTracker.Data.JobInfo;
using System.Data;
using static JobTracker.Data.DateInfo.ChangeData;
using static JobTracker.Data.DateInfo;
using System.Collections.ObjectModel;


namespace JobTracker.Data
{
    [Serializable]
    public class JobInfo
    {
        public const string c_JOBTITLE = "Job";
        public const string c_COMPANY = "Company";

        public enum Status
        { 
            Empty, //0
            Reasearching, //1
            WorkingOnMaterials, //2
            Applied, //3
            Interviewing, //4
            Accepted, //5
            Rejected //6
        }


        public Status status { get; set; }
        public string compoundTitle { get; set; }
        //main info
        public string jobTitle { get; set; }
        public string companyName { get; set; }
        public string URL { get; set; }

        public bool found { get; set; }
        public bool researched { get; set; }
        public bool coverLetter { get; set; }
        public bool resume { get; set; }
        public bool applied { get; set; }
        public bool interviewing { get; set; }

        public bool accepted { get; set; }
        public bool rejected { get; set; }
        public string contactInfo { get; set; }
        public string notes { get; set; }
        public DateTime? dateLocated { get; set; }
        public DateTime? dateMaterialsFinished { get; set; }
        public DateTime? dateApplied { get; set; }
        public DateTime? dateNextSteps { get; set; }

        public JobInfo()
        {
            jobTitle = c_JOBTITLE;
            companyName = c_COMPANY;

            URL = "";

            found = true;
            researched = false;
            coverLetter = false;
            resume = false;
            applied = false;
            interviewing = false;

            accepted = false;
            rejected = false;

            status = GetStatus(accepted, rejected, interviewing, applied, researched, coverLetter, resume);

            compoundTitle = GetCompoundTitle(companyName:companyName, jobTitle:jobTitle, status: status);

            contactInfo = "";
            notes = string.Empty;
        }

        public static string GetCompoundTitle(JobInfo jobInfo, bool includeStatus = true)
        {
            if (includeStatus)
            {
                return string.Format("{0} - {1} - {2}", jobInfo.companyName, jobInfo.jobTitle, GetStatusString(GetStatus(jobInfo)));
            }
            else
            {
                return string.Format("{0} - {1}", jobInfo.companyName, jobInfo.jobTitle);
            }
        }

        public static string GetCompoundTitle(string companyName, string jobTitle, Status status)
        {
            return string.Format("{0} - {1} - {2}", companyName, jobTitle, GetStatusString(status));
        }

        public void UpdateCompoundTitle()
        {
            compoundTitle = GetCompoundTitle(this);
        }

        private static Status GetStatus(JobInfo jobInfo)
        {
            var status = new Status();
            if (jobInfo.accepted)
            {
                status = Status.Accepted;
            }
            else if (jobInfo.rejected)
            {
                status = Status.Rejected;
            }
            else if (jobInfo.interviewing)
            {
                status = Status.Interviewing;
            }
            else if (jobInfo.applied)
            {
                status = Status.Applied;
            }
            else if (jobInfo.researched || jobInfo.coverLetter || jobInfo.resume)
            {
                status = Status.WorkingOnMaterials;
            }
            else
            {
                status = Status.Reasearching;
            }

            return status;
        }

        static public Status GetStatus(bool accepted, bool rejected, bool interviewing, bool applied, bool researched, bool coverLetter, bool resume)
        {
            var status = new Status();
            if (accepted) { status = Status.Accepted; }
            else if (rejected) { status = Status.Rejected; }
            else if (interviewing) { status = Status.Interviewing; }
            else if (applied) { status = Status.Applied; }
            else if (researched || coverLetter || resume) { status = Status.WorkingOnMaterials; }
            else { status = Status.Reasearching; }

            return status;
        }

        static private string GetStatusString(Status status)
        {
            var statusDictionary = new Dictionary<Status, string>()
            {
            {Status.Reasearching, "Researching" },
            {Status.WorkingOnMaterials, "Working On Materials" },
            {Status.Applied, "Applied" },
            {Status.Interviewing, "Interviewing" },
            {Status.Rejected, "Rejected" },
            {Status.Accepted, "Accepted" },
            {Status.Empty, "" },
            };

            return statusDictionary[status];
        }
    }

    public class DateInfo
    {
        public struct ChangeData
        {
            public DateTime ChangeDate { get; }
            public ChangeType TypeOfChange { get; }
            public string ChangeJob { get; }

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
                        ChangeType.Located, GetCompoundTitle(jobInfo, includeStatus:false)));
                }
                if (jobInfo.dateMaterialsFinished != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateMaterialsFinished ?? DateTime.MinValue,
                    ChangeType.MaterialsFinished, GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateApplied != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateApplied ?? DateTime.MinValue,
                    ChangeType.Applied, GetCompoundTitle(jobInfo, includeStatus: false)));
                }
                if (jobInfo.dateNextSteps != null)
                {
                    changeDatas.Add(new ChangeData(jobInfo.dateNextSteps ?? DateTime.MinValue,
                    ChangeType.NextSteps, GetCompoundTitle(jobInfo, includeStatus: false)));
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
