using FFmpeg.NET;
using System;
using System.Collections.Generic;
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
        public JobStatus Status { get; set; } = new JobStatus();
        public string Name { get; set; } = "Converting a file";
        private readonly MediaFile inputFile;
        private readonly MediaFile outputFile;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public ConvertJob(string inputPath, string outputPath)
        {
            inputFile = new MediaFile(inputPath);
            outputFile = new MediaFile(outputPath);
            Name = $"Converting {Path.GetFileName(inputPath)} to {Path.GetExtension(outputPath)}";
        }
        public async void Start()
        {
            var ffmpeg = new Engine("Dependencies/ffmpeg.exe");
            ffmpeg.Error += Ffmpeg_Error;
            ffmpeg.Progress += Ffmpeg_Progress;
            ffmpeg.Complete += Ffmpeg_Complete;
            await ffmpeg.ConvertAsync(inputFile, outputFile, cancellationTokenSource.Token);
        }

        private void Ffmpeg_Complete(object sender, FFmpeg.NET.Events.ConversionCompleteEventArgs e)
        {
            Status.State = JobState.Finished;
            Status.Progress = 100;
            JobStateChanged?.Invoke(null, EventArgs.Empty);
        }

        private void Ffmpeg_Progress(object sender, FFmpeg.NET.Events.ConversionProgressEventArgs e)
        {
            Status.State = JobState.Running;
            Status.Progress = 50;
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
