using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Artist
{
    public int Artistid { get; set; }

    public string? Artistname { get; set; }

    public string? Yearsactive { get; set; }

    public string? Description { get; set; }

    public string? Photopath { get; set; }

    public virtual ICollection<ArtistAlbum> ArtistAlbums { get; set; } = new List<ArtistAlbum>();

    public virtual ICollection<GenreArtist> GenreArtists { get; set; } = new List<GenreArtist>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
