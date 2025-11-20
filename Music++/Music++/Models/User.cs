using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public short Roleid { get; set; }

    public short Subscriptionid { get; set; }

    public DateOnly Registrationdate { get; set; }

    public DateOnly Lastlogindate { get; set; }

    public string Fullname { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual Role Role { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;
}
