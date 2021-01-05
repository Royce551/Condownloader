using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYoutubeDL;
using static NYoutubeDL.Helpers.Enums;

namespace Condownloader.Jobs
{
    public class DownloadJob : IJob
    {
        public EventHandler<JobErrorEventArgs> JobError { get; set; }
        public EventHandler JobStateChanged { get; set; }
        public LoggingManager Logs { get; set; }
        public JobStatus Status { get; set; } = new();
        public string Name { get; set; } = "Downloading a video";

        private readonly YoutubeDL youtube;
        private readonly string url;
        public DownloadJob(string url, string fileName, bool audioOnly, AudioFormat audioFormat, VideoFormat videoFormat)
        {
            this.url = url;
            youtube = new YoutubeDL();
            youtube.YoutubeDlPath = "Dependencies/youtube-dl.exe";

            youtube.Options.FilesystemOptions.Output = $"Downloads/{fileName}.%(ext)s";
            youtube.Options.PostProcessingOptions.ExtractAudio = audioOnly;
            if (audioOnly)
            {
                youtube.Options.PostProcessingOptions.EmbedThumbnail = true;
                youtube.Options.PostProcessingOptions.AddMetadata = true;
            }
            youtube.Options.PostProcessingOptions.AudioFormat = audioFormat;
            youtube.Options.VideoFormatOptions.Format = videoFormat;
        }
        public async void Start()
        {
            youtube.StandardErrorEvent += (object sender, string errorOutput) => 
            {
                Status.State = JobState.Failed;
                JobStateChanged?.Invoke(null, EventArgs.Empty);
                JobError?.Invoke(null, new JobErrorEventArgs { Error = errorOutput }); 
            };
            youtube.StandardOutputEvent += (object sender, string output) =>
            {
                Logs.WriteLog(output);
            };
            youtube.Info.PropertyChanged += delegate
            {
                Status.Progress = youtube.Info.VideoProgress;
                Name = $"Downloading \"{youtube.Info.Title}\"";
                if (Status.Progress == 100)
                {
                    Status.State = JobState.Finished;
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.Combine(Environment.CurrentDirectory, "Downloads"),
                        UseShellExecute = true
                    });
                }
                else Status.State = JobState.Running;
                JobStateChanged?.Invoke(null, EventArgs.Empty);
            };
            try
            {
                await youtube.DownloadAsync(url);
            }
            catch (TaskCanceledException) 
            { 
                // ignored 
            }
        }
        public void Stop() => youtube.CancelDownload();
    }
}
