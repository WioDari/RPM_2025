using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Media;
using Spotify.Context;
using Spotify.Models;

namespace Spotify;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void LogIn_Click(object sender, RoutedEventArgs e)
    {
        {
            using (var db = new PostgresContext())
            {
                string emailText = Email.Text ?? string.Empty;
                string passwordText = Password.Text ?? string.Empty;

                if (string.IsNullOrWhiteSpace(emailText) || string.IsNullOrWhiteSpace(passwordText))
                {
                    return;
                }

                var user = db.Users.FirstOrDefault(x =>
                    x.UserPassword == passwordText &&
                    x.UserLogin == emailText);

                if (user != null)
                {
                    var employee = db.Roles.FirstOrDefault(e => e.RoleId == user.UserId);

                    if (employee != null)
                    {
                        var position = db.Roles.FirstOrDefault(p => p.RoleId == employee.RoleId);

                        if (position?.RoleName == "Manager")
                        {
                            new Administrator().Show();
                        }
                        else if (position?.RoleName == "User")
                        {
                            new Administrator().Show();
                        }
                        else if (position?.RoleName == "Guest")
                        {
                            new Administrator().Show();
                        }
                        else if (position?.RoleName == "Guest")

                        {
                            new Administrator().Show();
                        }
                    }
                    else
                    {
                        new Administrator().Show();
                    }

                    this.Close();
                }
            }
        }
    }
}