using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condownloader
{
    public class LoggingManager
    {
        public List<string> Logs { get; private set; } = new();
        public EventHandler<string> NewLogsRecieved;
        public void WriteLog(string log)
        {
            Logs.Add(log);
            NewLogsRecieved?.Invoke(null, log);
        }
    }
}
