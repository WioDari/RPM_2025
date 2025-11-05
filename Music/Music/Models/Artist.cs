using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class Artist
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CountryId { get; set; }

    public string Description { get; set; } = null!;

    public int ActiveStartYear { get; set; }

    public int? ActiveEndYear { get; set; }

    public string? PhotoPath { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
