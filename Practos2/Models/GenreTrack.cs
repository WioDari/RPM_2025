using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class GenreTrack
{
    public int Genretrackid { get; set; }

    public int? Trackid { get; set; }

    public int? Genreid { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Track? Track { get; set; }
}
