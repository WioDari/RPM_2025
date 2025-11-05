using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class TracksGenre
{
    public int TrackGenreId { get; set; }

    public int? TrackId { get; set; }

    public int? GenreId { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Track? Track { get; set; }
}
