using FFmpeg.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Condownloader.Jobs
{
    class ConvertJob : IJob
    {
        public EventHandler<JobErrorEventArgs> JobError { get; set; }
        public EventHandler JobStateChanged { get; set; }
        public LoggingManager Logs { get; set; }
        public JobStatus Status { get; set; } = new();
        public string Name { get; set; } = "Converting a file";
        private readonly InputFile inputFile;
        private readonly OutputFile outputFile;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        public ConvertJob(string inputPath, string outputPath)
        {
            inputFile = new InputFile(inputPath);
            outputFile = new OutputFile(outputPath);
            Name = $"Converting {Path.GetFileName(inputPath)} to {Path.GetExtension(outputPath)}";
        }
        public async void Start()
        {
            var ffmpeg = new Engine("Dependencies/Windows/ffmpeg.exe"); // TODO: handle platform
            ffmpeg.Error += Ffmpeg_Error;
            ffmpeg.Progress += Ffmpeg_Progress;
            ffmpeg.Complete += Ffmpeg_Complete;
            await ffmpeg.ConvertAsync(inputFile, outputFile, cancellationTokenSource.Token);
        }

        private void Ffmpeg_Complete(object sender, FFmpeg.NET.Events.ConversionCompleteEventArgs e)
        {
            Status.State = JobState.Finished;
            Status.Progress = 100;
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.GetDirectoryName(outputFile.FileInfo.FullName),
                UseShellExecute = true
            });
            JobStateChanged?.Invoke(null, EventArgs.Empty);
        }

        private void Ffmpeg_Progress(object sender, FFmpeg.NET.Events.ConversionProgressEventArgs e)
        {
            Status.State = JobState.Running;
            Status.Progress = e.ProcessedDuration.TotalSeconds / e.TotalDuration.TotalSeconds;
            Status.ExtraInfo = $"Bitrate: {e.Bitrate}, Frame: {e.Frame}, FPS: {e.Fps}";
            JobStateChanged?.Invoke(null, EventArgs.Empty);
        }

        private void Ffmpeg_Error(object sender, FFmpeg.NET.Events.ConversionErrorEventArgs e)
        {
            JobStateChanged?.Invoke(null, EventArgs.Empty);
            JobError?.Invoke(null, new JobErrorEventArgs { Error = e.Exception.Message });
        }

        public void Stop() => cancellationTokenSource.Cancel();
    }
}