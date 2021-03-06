﻿using Condownloader.Jobs;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using static NYoutubeDL.Helpers.Enums;
using Condownloader.Configuration;

namespace Condownloader
{
    public partial class Form1 : Form
    {
        public JobManager JobManager = new();
        public LoggingManager LoggingManager = new();
        public ConfigurationFile Config;
        public Form1()
        {
            InitializeComponent();
            Config = ConfigurationManager.Read();
            RestoreSettings();
            JobManager.JobError += JobError;
            JobManager.JobStateChanged += JobStateChanged;
            // editing menu bar items in the designer seems to be screwed right now; for now, just manually subscribe to events
            ViewLogsMenuItem.Click += ViewLogsMenuItem_Click;
            ExitMenuItem.Click += ExitMenuItem_Click;
            AboutMenuItem.Click += AboutMenuItem_Click;
        }
        public void RestoreSettings()
        {
            DownloadUrlTextBox.Text = Config.DownloadURLS;
            DownloadFileNameTextBox.Text = Config.DownloadFileName;
            DownloadAudioOnlyCheckBox.Checked = Config.DownloadAudioOnly;
            SetAudioOptionsEnabled(Config.DownloadAudioOnly);
            ConvertFormatBox.Text = Config.ConvertFormat;
        }
        private void AboutMenuItem_Click(object sender, EventArgs e) => new AboutForm().Show();
        private void ExitMenuItem_Click(object sender, EventArgs e) => Application.Exit();
        private void ViewLogsMenuItem_Click(object sender, EventArgs e) => new LogViewer(LoggingManager).Show();

        private void JobError(object sender, JobErrorEventArgs args)
        {
            MessageBox.Show(args.Error);
        }
        private void JobStateChanged(object sender, EventArgs args)
        {
            
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
            foreach (var url in DownloadUrlTextBox.Text.Split(';'))
            {
                string fileName = DownloadFileNameTextBox.Text == string.Empty ? "file" : DownloadFileNameTextBox.Text;
                var job = new DownloadJob(url, fileName, DownloadAudioOnlyCheckBox.Checked, audioFormat, videoFormat);
                JobManager.AddJob(job, LoggingManager);
                job.Start();
            }
            timer1.Start();
        }
        private void ConvertButton_Click(object sender, EventArgs e)
        {
            var paths = ConvertInputTextBox.Text.Split(';');
            if (paths.Length > 0)
            {
                var result = MessageBox.Show("Starting multiple convert jobs at once will use a huge amount of resources and may render your PC unusable until they finish.",
                    "Condownloader", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel) return;
            }
            foreach (var path in paths)
            {
                var job = new ConvertJob(path, Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}{ConvertFormatBox.Text}"));
                JobManager.AddJob(job, LoggingManager);
                job.Start();
            }
            timer1.Start();
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

        private void label4_Click(object sender, EventArgs e) => 
            Process.Start(new ProcessStartInfo
            {
                FileName = @"https://github.com/ytdl-org/youtube-dl/blob/master/README.md#output-template",
                UseShellExecute = true
            });

        private void groupBox2_DragDrop(object sender, DragEventArgs e)
        {
            ConvertInputTextBox.Text = string.Join(';', e.Data.GetData(DataFormats.FileDrop) as string[]);
        }

        private void groupBox2_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1?.Items?.Clear();
            listBox1.BeginUpdate();
            foreach (var job in JobManager.Jobs)
            {
                listBox1.Items.Add($"{job.Name} - {job.Status.State} | {job.Status.Progress}%");
            }
            listBox1.EndUpdate();
            if (JobManager.Jobs.All(x => x.Status.Progress == 100)) timer1.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.DownloadURLS = DownloadUrlTextBox.Text;
            Config.DownloadFileName = DownloadFileNameTextBox.Text;
            Config.DownloadAudioOnly = DownloadAudioOnlyCheckBox.Checked;
            Config.ConvertFormat = ConvertFormatBox.Text;
            ConfigurationManager.Write(Config);
            if (JobManager.Jobs.Exists(x => x.Status.State == JobState.Running))
            {
                var result = MessageBox.Show("You still have running jobs. They'll be stopped when you close Condownloader.", "Condownloader", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result != DialogResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
            }
            foreach (var job in JobManager.Jobs)
            {
                job.Stop();
            }
            JobManager.Jobs.Clear();
        }
    }
}
