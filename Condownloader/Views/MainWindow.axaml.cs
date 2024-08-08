using Avalonia.Controls;

namespace Condownloader.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NativeMenuItem_Click(object? sender, System.EventArgs e)
        {
            new AboutWindow().Show();
        }
    }
}