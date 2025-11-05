using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class TracksInPlaylist
{
    public int TiPId { get; set; }

    public int TrackId { get; set; }

    public int PlaylistId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
