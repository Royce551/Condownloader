using Avalonia.Controls;
using Condownloader.ViewModels;

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
            new AboutWindow().Show();
        }

        private void NativeMenuItem_Click_1(object? sender, System.EventArgs e)
        {
            if (viewModel != null) new LogViewerWindow(viewModel.LoggingManager).Show();
        }
    }
}