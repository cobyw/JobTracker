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

namespace JobTracker.Data
{
    [Serializable]
    public class JobInfo
    {
        public const string c_JOBTITLE = "Job";
        public const string c_COMPANY = "Company";

        public enum Status
        { 
            Reasearching, //0
            WorkingOnMaterials, //1
            Applied, //2
            Interviewing, //3
            Accepted, //4
            Rejected //5
        } 

        public Status status { get; private set; }
        public string compoundTitle { get; private set; }
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
            UpdateStatus();
            jobTitle = c_JOBTITLE;
            companyName = c_COMPANY;
            UpdateCompoundTitle (jobTitle, companyName, this);

            URL = "";

            found = true;
            researched = false;
            coverLetter = false;
            resume = false;
            applied = false;
            interviewing = false;

            accepted = false;
            rejected = false;

            contactInfo = "";
            notes = string.Empty;
        }

        public static string UpdateCompoundTitle(string jobTitle, string companyName, JobInfo jobInfo = null)
        {
            var newTitle = string.Format("{0} - {1}", companyName, jobTitle);

            if (jobInfo != null)
            {
                jobInfo.compoundTitle = newTitle;
            }

            return newTitle;
        }

        public void UpdateStatus()
        {
            if (accepted) { status = Status.Accepted; }
            else if (rejected) { status = Status.Rejected; }
            else if (interviewing) { status = Status.Interviewing; }
            else if (applied) { status = Status.Applied; }
            else if (researched) {status = Status.WorkingOnMaterials; }
            else {status = Status.Reasearching; }
        }
    }
}
