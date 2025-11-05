using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Tag1 { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
