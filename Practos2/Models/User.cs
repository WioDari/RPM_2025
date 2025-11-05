using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class User
{
    public int Userid { get; set; }

    public string? Userlogin { get; set; }

    public string? Userpassword { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public int? Roleid { get; set; }

    public int? Subscriptionid { get; set; }

    public DateOnly? Regdate { get; set; }

    public int? Lastloginid { get; set; }

    public int? Countryid { get; set; }

    public virtual Country? Country { get; set; }

    public virtual LoginsTable? Lastlogin { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual Role? Role { get; set; }

    public virtual Subscription? Subscription { get; set; }

    public virtual ICollection<UserPlaylist> UserPlaylists { get; set; } = new List<UserPlaylist>();
}
