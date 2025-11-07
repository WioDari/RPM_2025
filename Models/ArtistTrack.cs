using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class ArtistTrack
{
    public int ArtistTracksId { get; set; }

    public int ArtistId { get; set; }

    public int TrackId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
