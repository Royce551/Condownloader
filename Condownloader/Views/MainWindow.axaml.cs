using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Condownloader.ViewModels;
using MsBox.Avalonia;
using SIADL.Avalonia;
using System;

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

        private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            viewModel.JobManager.JobError += JobError;
        }

        private void JobError(object sender, JobErrorEventArgs args)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Condownloader", args.Error, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error, WindowStartupLocation.CenterOwner);
                _ = box.ShowWindowDialogAsync(this);
            });
        }
    }
}