using JobTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Data;
using System.Runtime;
using static JobTracker.Data.Job;
using System.ComponentModel;

namespace JobTracker.ViewModel
{
    public class JobViewModel : ViewModelBase
    {

        public JobViewModel(Job job) {
            _job = job;
        }

        private readonly Job _job;

        public string CompoundTitle
        {
            get => string.Format("{0} - {1} - {2}", _job.companyName, _job.jobTitle, GetStatusString(_job.Status));
        }

    //properties
        public string CompanyName
    {
        get
        {
            return _job.companyName;
        }
        set
        {
                _job.companyName = value;
                OnPropertyChanged(nameof(CompanyName));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }

        public string JobTitle
        {
            get
            {
                return _job.jobTitle;
            }
            set
            {
                _job.jobTitle = value;
                OnPropertyChanged(nameof(JobTitle));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }

        public string URL
        {
            get
            {
                return _job.URL;
            }
            set
            {
                _job.URL = value;
                OnPropertyChanged(nameof(URL));
            }
        }

        public bool Found
        {
            get
            {
                return _job.found;
            }
            set
            {
                _job.found = value;
                OnPropertyChanged(nameof(Found));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }


        public bool Researched
        {
            get
            {
                return _job.researched;
            }
            set
            {
                _job.researched = value;
                OnPropertyChanged(nameof(Researched));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }

        public bool CoverLetter
        {
            get
            {
                return _job.coverLetter;
            }
            set
            {
                _job.coverLetter = value;
                OnPropertyChanged(nameof(CoverLetter));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }

        public bool Resume
        {
            get
            {
                return _job.resume;
            }
            set
            {
                _job.resume = value;
                OnPropertyChanged(nameof(Resume));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }

        public bool Applied
        {
            get
            {
                return _job.applied;
            }
            set
            {
                _job.applied = value;
                OnPropertyChanged(nameof(Applied));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }
        public bool Interviewing
        {
            get
            {
                return _job.interviewing;
            }
            set
            {
                _job.interviewing = value;
                OnPropertyChanged(nameof(Interviewing));
                OnPropertyChanged(nameof(CompoundTitle));
            }
        }

        public bool Pending
        {
            get
            {
                return _job.pending;
            }
            set
            {
                _job.pending = value;
                OnPropertyChanged(nameof(Pending));
            }
        }

        public bool Accepted
        {
            get
            {
                return _job.accepted;
            }
            set
            {
                _job.accepted = value;
                OnPropertyChanged(nameof(Accepted));
            }
        }

        public bool Rejected
        {
            get
            {
                return _job.rejected;
            }
            set
            {
                _job.rejected = value;
                OnPropertyChanged(nameof(Rejected));
            }
        }

        public string ContactInfo
        {
            get
            {
                return _job.contactInfo;
            }
            set
            {
                _job.contactInfo = value;
                OnPropertyChanged(nameof(ContactInfo));
            }
        }

        public DateTime? DateLocated
        {
            get
            {
                return _job.dateLocated;
            }
            set
            {
                _job.dateLocated = value;
                OnPropertyChanged(nameof(DateLocated));
            }
        }

        public DateTime? DateMaterialsFinished
        {
            get
            {
                return _job.dateMaterialsFinished;
            }
            set
            {
                _job.dateMaterialsFinished = value;
                OnPropertyChanged(nameof(DateMaterialsFinished));
            }
        }

        public DateTime? DateApplied
        {
            get
            {
                return _job.dateApplied;
            }
            set
            {
                _job.dateApplied = value;
                OnPropertyChanged(nameof(DateApplied));
            }
        }

        public DateTime? DateNextSteps
        {
            get
            {
                return _job.dateNextSteps;
            }
            set
            {
                _job.dateNextSteps = value;
                OnPropertyChanged(nameof(DateNextSteps));
            }
        }

        public string Notes
        {
            get
            {
                return _job.notes;
            }
            set
            {
                _job.notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
    }
}
