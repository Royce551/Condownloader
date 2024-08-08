using CommunityToolkit.Mvvm.ComponentModel;
using Condownloader.Jobs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condownloader.ViewModels
{
    public partial class LogViewerViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<string> logLines = new();

        private readonly LoggingManager loggingManager;
        public LogViewerViewModel(LoggingManager loggingManager)
        {
            this.loggingManager = loggingManager;

            loggingManager.NewLogsRecieved += LoggingManager_NewLogsRecieved;
            LogLines = new(loggingManager.Logs);
        }

        private void LoggingManager_NewLogsRecieved(object sender, string e) => LogLines.Add(e);

        public void ClearAllCommand()
        {
            LogLines.Clear();
        }

        public void SaveToFileCommand()
        {

        }
    }
}
