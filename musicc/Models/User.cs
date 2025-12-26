using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace musicc.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserPass { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public DateOnly Registration { get; set; }

    public DateTime? LastLog { get; set; }

    public bool? Ssubsription { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
