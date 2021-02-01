using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Condownloader;
using Condownloader.Configuration;
using Condownloader.Jobs;
using static NYoutubeDL.Helpers.Enums;

namespace Condownloader_Avalonia
{
    public class MainWindow : Window
    {
        public JobManager JobManager = new();
        public LoggingManager LoggingManager = new();
        public ConfigurationFile Config;

        private TextBox DownloadUrlTextBox;
        private TextBox DownloadFileNameTextBox;
        private CheckBox DownloadAudioOnlyCheckBox;
        private ComboBox DownloadFormatComboBox;
        private ComboBox comboBox1;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DownloadUrlTextBox = this.Find<TextBox>("DownloadUrlTextBox");
            DownloadFileNameTextBox = this.Find<TextBox>("DownloadFileNameTextBox");
            DownloadAudioOnlyCheckBox = this.Find<CheckBox>("DownloadAudioOnlyCheckBox");
            DownloadFormatComboBox = this.Find<ComboBox>("DownloadFormatComboBox");
            comboBox1 = this.Find<ComboBox>("comboBox1");

            Config = ConfigurationManager.Read();
            RestoreSettings();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public void RestoreSettings()
        {
            DownloadUrlTextBox.Text = Config.DownloadURLS;
            DownloadFileNameTextBox.Text = Config.DownloadFileName;
            DownloadAudioOnlyCheckBox.IsChecked = Config.DownloadAudioOnly;
            //SetAudioOptionsEnabled(Config.DownloadAudioOnly);
            //ConvertFormatBox.Text = Config.ConvertFormat;
        }
        public void DownloadButton_Click(object sender, RoutedEventArgs e)
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
                var job = new DownloadJob(url, fileName, (bool)DownloadAudioOnlyCheckBox.IsChecked, audioFormat, videoFormat);
                JobManager.AddJob(job, LoggingManager);
                job.Start();
            }
        }
    }
}
