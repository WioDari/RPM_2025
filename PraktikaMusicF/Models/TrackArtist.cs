using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class TrackArtist
{
    public int TrackArtId { get; set; }

    public int? TrackId { get; set; }

    public int? ArtId { get; set; }

    public virtual Artist? Art { get; set; }

    public virtual Track? Track { get; set; }
}
