using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Media;
using musicc.Context;
using musicc.Models;
using musicc.Models;

namespace musicc;

public partial class MainWindow : Window
{
    private string captch;
    public MainWindow()
    {
        InitializeComponent();
    }
    private void into_Click(object sender, RoutedEventArgs e)
{
    using (var bd = new PostgresContext())
    {
        var user = bd.Users.FirstOrDefault(u => u.UserLogin == box_name.Text && u.UserPass == passw_view.Text);

        if (user != null)
        {
            var window1 = new Window1();
            window1.Show();
            this.Close();
        }
        else
        {
            errors.Text = "Польз";
            return;
        }

    }
}

private void Button_Click(object sender, RoutedEventArgs e)
{

}



private void box_name_TextChanged(object sender, TextChangedEventArgs e)
{

}

private void Button_Click_1(object sender, RoutedEventArgs e)
{
    captcha();
}

private void Button_Click_2(object sender, RoutedEventArgs e)
{

}
public void captcha()
{
    var rand = new Random();
    const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz123456789";
    captch = new string(Enumerable.Repeat(chars, 6)
        .Select(s => s[rand.Next(s.Length)]).ToArray());
    capt.Text = captch;
    capt.RenderTransform = new RotateTransform(rand.Next(-10, 10));
    line_one.StartPoint = new Point(10, 10);
    line_one.EndPoint = new Point(10, 20);

}
}