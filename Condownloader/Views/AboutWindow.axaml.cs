using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SIADL.Avalonia;

namespace Condownloader;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SIADLUtilities.OpenURL(@"https://github.com/Royce551/Condownloader/releases");
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SIADLUtilities.OpenURL(@"https://github.com/Royce551/Condownloader/issues");
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SIADLUtilities.OpenURL(@"https://github.com/Royce551/Condownloader");
    }
}