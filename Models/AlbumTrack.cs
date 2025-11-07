using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class AlbumTrack
{
    public int AlbumTrackId { get; set; }

    public int AlbumId { get; set; }

    public int TrackId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
