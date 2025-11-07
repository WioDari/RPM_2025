using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class ArtistsInTrack
{
    public int AiTId { get; set; }

    public int ArtistId { get; set; }

    public int TrackId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
