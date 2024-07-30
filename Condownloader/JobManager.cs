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
        public JobState State { get; set; } = JobState.Running;
        public string ExtraInfo { get; set; } = string.Empty;
        public double Progress { get; set; } = 0;

        public bool IsRunning => State == JobState.Running;
        public bool IsFinished => State == JobState.Finished;
        public bool IsFailed => State == JobState.Failed;
    }
    public enum JobState
    {
        Finished,
        Running,
        Failed
    }
}