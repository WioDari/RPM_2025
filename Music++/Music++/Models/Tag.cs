using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Tag
{
    public short Tagid { get; set; }

    public string Tagname { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
