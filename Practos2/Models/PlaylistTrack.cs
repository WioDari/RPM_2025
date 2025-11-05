using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class PlaylistTrack
{
    public int Playlisttracksid { get; set; }

    public int? Playlistid { get; set; }

    public int? Trackid { get; set; }

    public virtual Playlist? Playlist { get; set; }

    public virtual Track? Track { get; set; }
}
