using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public int SubscriptionId { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public DateOnly LastLogin { get; set; }

    public virtual ICollection<PlaylistUser> PlaylistUsers { get; set; } = new List<PlaylistUser>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual Role Role { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;
}
