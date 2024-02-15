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
        public bool pending { get; set; }
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
            pending = true;

            status = GetStatus(this);

            compoundTitle = GetCompoundTitle(this, includeStatus:true);

            contactInfo = "";
            notes = string.Empty;
        }

        /// <summary>
        /// Returns the compound title of the current job including the company name, job title
        /// </summary>
        /// <param name="jobInfo">The job title whose title is being requested</param>
        /// <param name="includeStatus">Status is appended to the end of the title when true</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the stored compound title
        /// </summary>
        public void UpdateCompoundTitle()
        {
            compoundTitle = GetCompoundTitle(this);
        }

        /// <summary>
        /// Returns the status of the job based on current boxes checked
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Translates a status enum into a nicely formatted string
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
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
}
