using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class GenresInArtist
{
    public int GiAId { get; set; }

    public int? GenreId { get; set; }

    public int? ArtistId { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual Genre? Genre { get; set; }
}
