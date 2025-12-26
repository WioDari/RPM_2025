using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserLogin { get; set; } = null!;

    public string UserPwd { get; set; } = null!;

    public string? UserEmail { get; set; }

    public int? RoleId { get; set; }

    public int? SubscriptionId { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public DateOnly? LastentryDate { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual Roles Roles { get; set; }

    public virtual Subscription? Subscription { get; set; }
}
