using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Tag
{
    public int Tagid { get; set; }

    public string? Tagname { get; set; }

    public virtual ICollection<TagPlaylist> TagPlaylists { get; set; } = new List<TagPlaylist>();
}
