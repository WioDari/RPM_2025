using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class TrackArtist
{
    public int TrackArtistId { get; set; }

    public int TrackId { get; set; }

    public int ArtistId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
