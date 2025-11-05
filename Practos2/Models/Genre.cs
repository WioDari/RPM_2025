using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Genre
{
    public int Genreid { get; set; }

    public string? Genrename { get; set; }

    public virtual ICollection<GenreArtist> GenreArtists { get; set; } = new List<GenreArtist>();

    public virtual ICollection<GenreTrack> GenreTracks { get; set; } = new List<GenreTrack>();
}
