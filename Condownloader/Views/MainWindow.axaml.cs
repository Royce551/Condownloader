using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Condownloader.ViewModels;
using SIADL.Avalonia;

namespace Condownloader.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? viewModel => DataContext as MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NativeMenuItem_Click(object? sender, System.EventArgs e)
        {
            new AboutWindow().Show(this);
        }

        private void NativeMenuItem_Click_1(object? sender, System.EventArgs e)
        {
            if (viewModel != null) new LogViewerWindow(viewModel.LoggingManager).Show(this);
        }

        private void TextBlock_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            SIADLUtilities.OpenURL(@"https://github.com/ytdl-org/youtube-dl/blob/master/README.md#output-template");
        }

        private void NativeMenuItem_Click_2(object? sender, System.EventArgs e)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                desktopLifetime.Shutdown();
        }
    }
}