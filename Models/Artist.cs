using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public int? ArtistCountry { get; set; }

    public string ActiveYears { get; set; } = null!;

    public string ArtistDescription { get; set; } = null!;

    public string PhotoPath { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Country? ArtistCountryNavigation { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
