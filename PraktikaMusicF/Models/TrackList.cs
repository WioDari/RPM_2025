using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class TrackList
{
    public int TrackListId { get; set; }

    public int? AlbId { get; set; }

    public int? TrackId { get; set; }

    public virtual Album? Alb { get; set; }

    public virtual Track? Track { get; set; }
}
