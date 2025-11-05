using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual ICollection<AlbumGenre> AlbumGenres { get; set; } = new List<AlbumGenre>();

    public virtual ICollection<GenreArtist> GenreArtists { get; set; } = new List<GenreArtist>();

    public virtual ICollection<GenreTrack> GenreTracks { get; set; } = new List<GenreTrack>();
}
