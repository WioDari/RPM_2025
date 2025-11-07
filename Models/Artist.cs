using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string YearsActive { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PhotoPath { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<ArtistTrack> ArtistTracks { get; set; } = new List<ArtistTrack>();

    public virtual ICollection<GenreArtist> GenreArtists { get; set; } = new List<GenreArtist>();
}
