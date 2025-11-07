using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class GenreTrack
{
    public int GenreTrackId { get; set; }

    public int TrackId { get; set; }

    public int? GenreId { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Track Track { get; set; } = null!;
}
