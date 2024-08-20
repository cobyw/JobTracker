using System.Collections.ObjectModel;

namespace JobTracker.Model
{
    /// <summary>
    /// ChangeData includes all of the information about the type of change that occurred in a job.
    /// For example, when a job was applied to.
    /// </summary>
    public class ChangeData
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
        public string Job
        {
            get;
        }

        /// <summary>
        /// The name of the job that was changed
        /// </summary>
        public string Company
        {
            get;
        }

        /// <summary>
        /// Returns a politely formated version of the change type
        /// </summary>
        /// <param name="changeType">The type of change to be translated</param>
        /// <returns>The nicely formated change</returns>
        public string ChangeTypeString
        {
            get => changeTypeDictionary[TypeOfChange];
        }

        public ChangeData(DateTime changeDate, ChangeType typeOfChange, string company, string job)
        {
            ChangeDate = changeDate;
            TypeOfChange = typeOfChange;
            Company = company;
            Job = job;
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
    }
}