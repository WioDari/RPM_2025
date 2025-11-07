using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class GenersInTrack
{
    public int GiTId { get; set; }

    public int GenreId { get; set; }

    public int TrackId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
