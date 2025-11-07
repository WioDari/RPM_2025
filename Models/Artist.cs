using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public string YearsActivity { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PhotoPath { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<GenreArtist> GenreArtists { get; set; } = new List<GenreArtist>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
