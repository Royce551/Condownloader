using Condownloader.Jobs;
using System;
using System.Windows.Forms;
using static NYoutubeDL.Helpers.Enums;

namespace Condownloader
{
    public partial class Form1 : Form
    {
        public JobManager JobManager = new JobManager();
        public Form1()
        {
            InitializeComponent();
            JobManager.JobError += JobError;
            JobManager.JobStateChanged += JobStateChanged;
        }
        private void JobError(object sender, JobErrorEventArgs args)
        {
            MessageBox.Show(args.Error);
        }
        private void JobStateChanged(object sender, EventArgs args)
        {
            listBox1.Items.Clear();
            foreach (var job in JobManager.Jobs)
            {
                listBox1.Items.Add($"{job.Name} - {job.Status.State} | {job.Status.Progress}%");
            }
        }
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            var audioFormat = DownloadFormatComboBox.SelectedIndex switch
            {
                0 => AudioFormat.aac,
                1 => AudioFormat.best,
                2 => AudioFormat.m4a,
                3 => AudioFormat.mp3,
                4 => AudioFormat.opus,
                5 => AudioFormat.threegp,
                6 => AudioFormat.vorbis,
                7 => AudioFormat.wav,
                -1 or _ => AudioFormat.mp3
            };
            var videoFormat = comboBox1.SelectedIndex switch
            {
                0 => VideoFormat.undefined,
                1 => VideoFormat.mp4,
                2 => VideoFormat.flv,
                3 => VideoFormat.ogg,
                4 => VideoFormat.webm,
                5 => VideoFormat.mkv,
                6 => VideoFormat.avi,
                7 => VideoFormat.best,
                8 => VideoFormat.worst,
                -1 or _ => VideoFormat.mp4
            };
            string fileName = DownloadFileNameTextBox.Text == string.Empty ? "file" : DownloadFileNameTextBox.Text;
            var job = new DownloadJob(DownloadUrlTextBox.Text, fileName, DownloadAudioOnlyCheckBox.Checked, audioFormat, videoFormat);
            JobManager.AddJob(job);
            job.Start();
        }
        private void SetAudioOptionsEnabled(bool enabled)
        {
            DownloadFormatComboBox.Enabled = enabled;
            label2.Enabled = enabled;
            label3.Enabled = !enabled;
            comboBox1.Enabled = !enabled;
        }
        private void DownloadAudioOnlyCheckBox_CheckedChanged(object sender, EventArgs e) => SetAudioOptionsEnabled(DownloadAudioOnlyCheckBox.Checked);

        private void StopAllJobsButton_Click(object sender, EventArgs e)
        {
            foreach (var job in JobManager.Jobs)
            {
                job.Stop();
            }
            JobManager.Jobs.Clear();
            JobStateChanged(null, EventArgs.Empty);
        }
    }
}
