using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Data
{

    public struct JobInfo
    {
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
        public DateTime dateLocated
        {
            get; set;
        }
        public DateTime dateMaterialsFinished
        {
            get; set;
        }
        public DateTime dateApplied
        {
            get; set;
        }
        public DateTime dateNextSteps
        {
            get; set;
        }

        public JobInfo(string newJobTitle = "Job")
        {
            jobTitle = newJobTitle;
            companyName = "Company";
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

            /*
            dateLocated = DateTime.MinValue;
            dateMaterialsFinished = DateTime.MinValue;
            dateApplied = DateTime.MinValue;
            dateNextSteps = DateTime.MinValue;
            */
        }

        public override string ToString()
        {
            return jobTitle;
        }
    }
}
