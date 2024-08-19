using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Model
{
    public class JobManager
    {
        private static ObservableCollection<Job> _DataBaseJobs = new ObservableCollection<Job>();

        public static ObservableCollection<Job> GetJobs()
        {
            return _DataBaseJobs;
        }

        public static void AddJob(Job job)
        {
            _DataBaseJobs.Add(job);
        }

        public static void AddNewJob()
        {
            _DataBaseJobs.Add(new Job());
        }

        public static void RemoveJobAtIndex(int index)
        {
            _DataBaseJobs.RemoveAt(index);
        }

        public static void ClearJobs()
        {
            _DataBaseJobs.Clear();
        }

        public static int JobCount()
        {
            return _DataBaseJobs.Count;
        }
    }
}
