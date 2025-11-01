using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string NameTag { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
