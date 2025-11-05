using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class TagPlaylist
{
    public int Tagplaylistid { get; set; }

    public int? Playlistid { get; set; }

    public int? Tagid { get; set; }

    public virtual Playlist? Playlist { get; set; }

    public virtual Tag? Tag { get; set; }
}
