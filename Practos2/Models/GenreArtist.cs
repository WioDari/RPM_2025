using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class GenreArtist
{
    public int Genreartistid { get; set; }

    public int? Artistid { get; set; }

    public int? Genreid { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual Genre? Genre { get; set; }
}
