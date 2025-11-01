using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public int? PlaylistId { get; set; }

    public int SubscriptionId { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public DateTime LastLogin { get; set; }

    public bool IsBan { get; set; }

    public virtual Rol Role { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
