using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class Artist
{
    public int ArtistsId { get; set; }

    public string ArtistName { get; set; } = null!;

    public int? CountryId { get; set; }

    public string YearsActive { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? PhotoPath { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Country? Country { get; set; }

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
