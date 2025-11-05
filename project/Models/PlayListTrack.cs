using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class PlayListTrack
{
    public int PlayListTrackId { get; set; }

    public int PlaylistId { get; set; }

    public int TrackId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
