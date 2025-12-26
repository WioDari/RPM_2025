using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BelokonevPRM.Context;
using BelokonevPRM.Models;
using Microsoft.EntityFrameworkCore;

namespace BelokonevPRM.Windows;

public partial class MainWindow : Window
{
    User _user;
    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    public MainWindow(User user)
    {
        InitializeComponent();
        Load(user);
        _user = user;
        DataContext = this;
    }

    private void Load(User user)
    {
        using (var postgresContext = new PostgresContext())
        {
            NameBox.Text = user.UserName;
            RoleBox.Text = postgresContext.Roles.FirstOrDefault(x => x.RoleId == user.RoleId)?.Role;
        }
    }

    private async void ExitFromAccountOnClick(object? sender, RoutedEventArgs e)
    {
        using (var postgresContext = new PostgresContext())
        {
            postgresContext.Users
                .Where(u => u.UserLogin == _user.UserLogin)
                .ExecuteUpdateAsync(setters
                    => setters.SetProperty(u => u.LastentryDate, DateOnly.FromDateTime(DateTime.UtcNow)));
            await postgresContext.SaveChangesAsync();
            
            new LogWindow().Show();
        }
        Close();
    }
}

