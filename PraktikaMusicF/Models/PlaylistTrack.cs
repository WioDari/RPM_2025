using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class PlaylistTrack
{
    public int PlTrackId { get; set; }

    public int? PlId { get; set; }

    public int? TrackId { get; set; }

    public virtual Playlist? Pl { get; set; }

    public virtual Track? Track { get; set; }
}
