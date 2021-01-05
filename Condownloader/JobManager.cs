using Condownloader.Jobs;
using System;
using System.Collections.Generic;

namespace Condownloader
{
    public class JobManager
    {
        public List<IJob> Jobs { get; private set; } = new();
        public EventHandler<JobErrorEventArgs> JobError;
        public EventHandler JobStateChanged;

        public void AddJob(IJob job, LoggingManager logs)
        {
            job.JobError = JobError;
            job.JobStateChanged = JobStateChanged;
            job.Logs = logs;
            Jobs.Add(job);
        }
    }
    public class JobErrorEventArgs : EventArgs
    {
        public string Error { get; set; }
    }
    public class JobStatus
    {
        public JobState State { get; set; }
        public int Progress { get; set; }
    }
    public enum JobState
    {
        Finished,
        Running,
        Failed
    }
}
