using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Condownloader.Jobs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using static NYoutubeDL.Helpers.Enums;

namespace Condownloader.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public JobManager JobManager = new();
        public LoggingManager LoggingManager = new();

        private DispatcherTimer updateTimer = new();

        public MainWindowViewModel()
        {
            JobManager.JobError += JobError;
            updateTimer.Interval = TimeSpan.FromMilliseconds(100);
            updateTimer.Tick += UpdateTimer_Tick;
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e) => UpdateRunningJobsList();

        public void UpdateRunningJobsList()
        {
            if (JobManager.Jobs.Count <= 0 || JobManager.Jobs.All(x => x.Status.State == JobState.Finished)) updateTimer.Stop();

            RunningJobs = new(JobManager.Jobs);
        }

        private void JobError(object sender, EventArgs args)
        {

        }

        [ObservableProperty]
        private ObservableCollection<IJob> runningJobs = new();

        [ObservableProperty]
        private string downloadURL = string.Empty;

        [ObservableProperty]
        private string downloadFileName = string.Empty;

        [ObservableProperty]
        private bool downloadAudioOnly = false;

        [ObservableProperty]
        private int downloadVideoFormatIndex = 2;

        [ObservableProperty]
        private int downloadAudioFormatIndex = 1;

        public void StartDownloadJob()
        {
            var audioFormat = DownloadAudioFormatIndex switch
            {
                0 => AudioFormat.best,
                1 => AudioFormat.mp3,
                2 => AudioFormat.aac,
                3 => AudioFormat.m4a,
                4 => AudioFormat.opus,
                5 => AudioFormat.threegp,
                6 => AudioFormat.vorbis,
                7 => AudioFormat.wav,
                -1 or _ => AudioFormat.mp3
            };
            var videoFormat = DownloadVideoFormatIndex switch
            {
                0 => VideoFormat.best,
                1 => VideoFormat.worst,
                2 => VideoFormat.mp4,
                3 => VideoFormat.flv,
                4 => VideoFormat.ogg,
                5 => VideoFormat.webm,
                6 => VideoFormat.mkv,
                7 => VideoFormat.avi,
                -1 or _ => VideoFormat.mp4
            };
            foreach (var url in DownloadURL.Split(';'))
            {
                string fileName = DownloadFileName == string.Empty ? "file" : DownloadFileName;
                var job = new DownloadJob(url, fileName, DownloadAudioOnly, audioFormat, videoFormat);
                JobManager.AddJob(job, LoggingManager);
                job.Start();
            }
            updateTimer.Start();
        }

        [ObservableProperty]
        private string convertURL = string.Empty;

        [ObservableProperty]
        private string convertFormat = string.Empty;

        public void StartConvertJob()
        {
            var paths = ConvertURL.Split(';');
            foreach (var path in paths)
            {
                var job = new ConvertJob(path, Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}{ConvertFormat}"));
                JobManager.AddJob(job, LoggingManager);
                job.Start();
            }
            updateTimer.Start();
        }

        public void StopAllJobs()
        {
            foreach (var job in JobManager.Jobs)
            {
                job.Stop();
            }
            JobManager.Jobs.Clear();

            UpdateRunningJobsList();
        }
    }
}
