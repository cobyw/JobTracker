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
using static JobTracker.Model.Job;
using System.Data;
using static JobTracker.Model.Date.ChangeData;
using static JobTracker.Model.Date;
using System.Collections.ObjectModel;
using System.Security.RightsManagement;
using System.Runtime;


namespace JobTracker.Model
{
    [Serializable]
    public class Job
    {
        public const string c_JOBTITLE = "Job";
        public const string c_COMPANY = "Company";

        public enum JobStatus
        { 
            Empty, //0
            Reasearching, //1
            WorkingOnMaterials, //2
            Applied, //3
            Interviewing, //4
            Accepted, //5
            Rejected //6
        }

        /// <summary>
        /// A nicely formated dictionary of the statuses
        /// </summary>
        private static Dictionary<JobStatus, string> _statusDictionary = new Dictionary<JobStatus, string>()
            {
            {JobStatus.Reasearching, "Researching" },
            {JobStatus.WorkingOnMaterials, "Working On Materials" },
            {JobStatus.Applied, "Applied" },
            {JobStatus.Interviewing, "Interviewing" },
            {JobStatus.Rejected, "Rejected" },
            {JobStatus.Accepted, "Accepted" },
            {JobStatus.Empty, "" },
            };


        /// <summary>
        /// Translates a status enum into a nicely formatted string
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetStatusString(JobStatus status)
        {
            return _statusDictionary[status];
        }

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

        public Job()
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

            contactInfo = "";
            notes = string.Empty;
        }

        /*
        /// <summary>
        /// Returns the compound title of the current job including the company name, job title
        /// </summary>
        /// <param name="jobInfo">The job title whose title is being requested</param>
        /// <param name="includeStatus">Status is appended to the end of the title when true</param>
        /// <returns></returns>
        public string GetCompoundTitle(bool includeStatus = true)
        {
            if (includeStatus)
            {
                return string.Format("{0} - {1} - {2}", companyName, jobTitle, GetStatusString(Status));
            }
            else
            {
                return string.Format("{0} - {1}", companyName, jobTitle);
            }
        }
        */

        public JobStatus Status
        {
            get
            {
                var status = new JobStatus();
                if (accepted)
                {
                    status = JobStatus.Accepted;
                }
                else if (rejected)
                {
                    status = JobStatus.Rejected;
                }
                else if (interviewing)
                {
                    status = JobStatus.Interviewing;
                }
                else if (applied)
                {
                    status = JobStatus.Applied;
                }
                else if (researched || coverLetter || resume)
                {
                    status = JobStatus.WorkingOnMaterials;
                }
                else
                {
                    status = JobStatus.Reasearching;
                }

                return status;
            } 

        }
    }
}
