using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MusicPlusPlus;

public partial class MessageWindow : Window
{
    public MessageWindow()
    {
        InitializeComponent();
    }

    public MessageWindow(string title, string text)
    {
        InitializeComponent();
        TitleTBlock.Text = title;
        WindowContentTBlock.Text = text;
    }

    private void OKB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}