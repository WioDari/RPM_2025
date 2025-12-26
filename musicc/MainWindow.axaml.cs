using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Media;
using Microsoft.EntityFrameworkCore;
using musicc.Context;
using musicc.Models;
using musicc.Models;

namespace musicc;

public partial class MainWindow : Window
{
    private string captch;
    private int kd;
    private bool block = false;
    
    
    public MainWindow()
    {
        InitializeComponent();
        captcha();        
        
    }

    private async void into_Click(object sender, RoutedEventArgs e)
    {
        
        if (block == true)
        {
            errors.Text = "БАН";
            return;
        }

        if (kd == 5)
        {
            block = true;
            await Task.Delay(60000);
            errors.Text = "";
            block = false;
            kd = 0;
            captcha();
            return;
        }
      using (var bd = new PostgresContext())
        {
            var user = bd.Users.Include(u => u.Role).FirstOrDefault(u => u.UserLogin == box_name.Text);

            if (user == null)
            {
                errors.Text = "error";
                kd += 1;
                
                return;
            }
            if (user.UserPass == passw_view.Text)
            {
                if (user.RoleId == 1 && exit_capt.Text == captch)
                {
                    user.LastLog = DateTime.UtcNow;
                    bd.Users.Update(user);
                    await bd.SaveChangesAsync();
                    var wind = new Window1();
                    wind.Show();
                    this.Close();
                }
                else if (user.RoleId == 2 && exit_capt.Text == captch)
                {
                    user.LastLog = DateTime.UtcNow;
                    bd.Users.Update(user);
                    await bd.SaveChangesAsync();
                    var wind = new Window2();
                    wind.Show();
                    this.Close();
                }
                else if (user.RoleId == 4 && exit_capt.Text == captch)
                {
                    user.LastLog = DateTime.UtcNow;
                    bd.Users.Update(user);
                    await bd.SaveChangesAsync();
                    var wind = new Window3();
                    wind.Show();
                    this.Close();
                }
                else
                {
                    errors.Text = "ошибка капчи";
                    kd += 1;
                    return;
                }
            }
            else
            {
                errors.Text = "ошибка лог или пароля";  
                kd += 1;
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
        
    capt.RenderTransform = new RotateTransform(rand.Next(-30, 30));
        
    line_one.StartPoint = new Avalonia.Point(10, 10);
    line_one.EndPoint = new Avalonia.Point(50, 50);
    line_one.Stroke = new SolidColorBrush(Color.FromArgb(
        (byte)rand.Next(130, 200),
        (byte)rand.Next(256),
        (byte)rand.Next(256),
        (byte)rand.Next(256)
    ));
    line_one.StrokeThickness = 4;
        
    line_two.StartPoint = new Avalonia.Point(10, 50);
    line_two.EndPoint = new Avalonia.Point(50, 10);
    line_two.Stroke = new SolidColorBrush(Color.FromArgb(
        (byte)rand.Next(200, 256),
        (byte)rand.Next(256),
        (byte)rand.Next(256),
        (byte)rand.Next(256)
    ));
    line_two.StrokeThickness = 5;
}

}