using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
