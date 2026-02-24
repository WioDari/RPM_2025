using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public string TrackName { get; set; } = null!;

    public int? Artist { get; set; }

    public string? Info { get; set; }

    public TimeSpan? TrackDuration { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public int Bitrate { get; set; }

    public string TrackPath { get; set; } = null!;

    public string AlbumcoverPath { get; set; } = null!;

    public decimal? TrackRating { get; set; }

    public int? TotalPlays { get; set; }

    public virtual Artist? ArtistNavigation { get; set; }
}
