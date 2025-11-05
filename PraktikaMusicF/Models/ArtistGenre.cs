using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class ArtistGenre
{
    public int ArtGenreId { get; set; }

    public int? ArtId { get; set; }

    public int? GenreId { get; set; }

    public virtual Artist? Art { get; set; }

    public virtual Genre? Genre { get; set; }
}
