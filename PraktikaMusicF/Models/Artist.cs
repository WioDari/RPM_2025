using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class Artist
{
    public int ArtId { get; set; }

    public string? ArtName { get; set; }

    public string? YearActive { get; set; }

    public string? ArtDesc { get; set; }

    public string? ArtPhotoPath { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<ArtistGenre> ArtistGenres { get; set; } = new List<ArtistGenre>();

    public virtual ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();
}
