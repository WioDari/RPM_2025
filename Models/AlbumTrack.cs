using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class AlbumTrack
{
    public int AlbumTracksId { get; set; }

    public int AlbumId { get; set; }

    public int TracksId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Track Tracks { get; set; } = null!;
}
