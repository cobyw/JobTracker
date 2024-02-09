using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Markup;
using System.Runtime.Serialization;

namespace JobTracker.Data
{
    [Serializable]
    public class JobInfo
    {
        public const string c_JOBTITLE = "Job";
        public const string c_COMPANY = "Company";

        public string compoundTitle
        { get; set; }

        //main info
        public string jobTitle
        {
            get; set;
        }
        public string companyName
        {
            get; set;
        }
        public string URL
        {
            get; set;
        }

        public bool found
        {
            get; set;
        }
        public bool researched
        {
            get; set;
        }
        public bool coverLetter
        {
            get; set;
        }
        public bool resume
        {
            get; set;
        }
        public bool applied
        {
            get; set;
        }
        public bool interviewing
        {
            get; set;
        }

        public bool accepted
        {
            get; set;
        }
        public bool rejected
        {
            get; set;
        }

        public string contactInfo
        {
            get; set;
        }
        public string notes
        {
            get; set;
        }
        public DateTime? dateLocated
        {
            get; set;
        }
        public DateTime? dateMaterialsFinished
        {
            get; set;
        }
        public DateTime? dateApplied
        {
            get; set;
        }
        public DateTime? dateNextSteps
        {
            get; set;
        }

        public JobInfo()
        {
            jobTitle = c_JOBTITLE;
            companyName = c_COMPANY;
            compoundTitle = string.Format("{0} - {1}", companyName, jobTitle);
            URL = "URL";

            found = true;
            researched = false;
            coverLetter = false;
            resume = false;
            applied = false;
            interviewing = false;

            accepted = false;
            rejected = false;

            contactInfo = "Contact name and info";
            notes = string.Empty;
        }
    }
}
