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
