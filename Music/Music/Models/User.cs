using System;
using System.Collections.Generic;
using System.Windows;

namespace Music.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public int SubscriptionId { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public DateTime? LastLoginDateTime { get; set; }

    public TimeSpan BanInterval { get; set; }

    public DateTime BanDateTime { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual UserRole Role { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;

    public virtual ICollection<Playlist> PlaylistsNavigation { get; set; } = new List<Playlist>();

    public override string ToString()
    {
        return Login;
    }

    public virtual string UnbanTime
    {
        get
        {
            var timeSpan = BanDateTime + BanInterval - DateTime.Now;
            if (timeSpan > TimeSpan.Zero)
                return $"{(int)timeSpan.TotalMinutes} мин {timeSpan.Seconds} сек";
            else
                return "Нет блокировки";
        }
    }

    public virtual Visibility UnbanButtonVisibility
    {
        get
        {
            if (RoleId == 1)
                return Visibility.Hidden;
            else
                return (BanDateTime + BanInterval - DateTime.Now) > TimeSpan.Zero ? Visibility.Visible : Visibility.Hidden;
        }
    }
    public virtual Visibility BanButtonVisibility
    {
        get
        {
            if (RoleId == 1)
                return Visibility.Hidden;
            else
                return (BanDateTime + BanInterval - DateTime.Now) > TimeSpan.Zero ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
