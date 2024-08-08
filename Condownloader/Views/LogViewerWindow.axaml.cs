using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Condownloader.ViewModels;

namespace Condownloader;

public partial class LogViewerWindow : Window
{
    public LogViewerWindow()
    {
        InitializeComponent();
    }

    public LogViewerWindow(LoggingManager loggingManager)
    {
        InitializeComponent();
        DataContext = new LogViewerViewModel(loggingManager);
    }
}