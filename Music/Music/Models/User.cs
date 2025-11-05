using System;
using System.Collections.Generic;

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

    public DateOnly? LastLoginDate { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual UserRole Role { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;

    public virtual ICollection<Playlist> PlaylistsNavigation { get; set; } = new List<Playlist>();
}
