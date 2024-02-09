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

        public static string GetCompoundTitle(string companyName, string jobTitle, Status status)
        {
            return string.Format("{0} - {1} - {2}", companyName, jobTitle, GetStatusString(status));
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
            };

            return statusDictionary[status];
        }
    }
}
