using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condownloader.Jobs
{
    public interface IJob
    {
        public EventHandler<JobErrorEventArgs> JobError { get; set; }
        public EventHandler JobStateChanged { get; set; }
        public LoggingManager Logs { get; set; }
        public JobStatus Status { get; set; }
        public string Name { get; set; }
        void Start();
        void Stop();
    }
}
