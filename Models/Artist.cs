using System;
using System.Collections.Generic;

namespace Spotify.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public int YearsActive { get; set; }

    public string? Description { get; set; }

    public string? PhotoPath { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
