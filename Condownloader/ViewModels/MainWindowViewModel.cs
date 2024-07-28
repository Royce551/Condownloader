using CommunityToolkit.Mvvm.ComponentModel;

namespace Condownloader.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
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

        }

        [ObservableProperty]
        private string convertURL = string.Empty;

        [ObservableProperty]
        private string convertFormat = string.Empty;

        public void StartConvertJob()
        {

        }
    }
}
