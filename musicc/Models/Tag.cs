using System;
using System.Collections.Generic;

namespace musicc.Models;

public partial class Tag
{
    public int TagsId { get; set; }

    public string? Tag1 { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
