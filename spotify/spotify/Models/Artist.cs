using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class Artist
{
    public int Id { get; set; }

    public string ArtistName { get; set; } = null!;

    public int CountryId { get; set; }

    public string Description { get; set; } = null!;

    public string PhotoPath { get; set; } = null!;

    public int StartYearActive { get; set; }

    public int EndYearActive { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Countrye Country { get; set; } = null!;

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
