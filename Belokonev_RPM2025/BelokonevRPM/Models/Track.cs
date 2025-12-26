using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public string TrackName { get; set; } = null!;

    public int? Album { get; set; }

    public string Duration { get; set; } = null!;

    public int Bitrate { get; set; }

    public double? Rating { get; set; }

    public long? Playcount { get; set; }

    public DateOnly Realeasedate { get; set; }

    public string? CoverPath { get; set; }

    public virtual Album? AlbumNavigation { get; set; }

    public virtual ICollection<Playlisttrack> Playlisttracks { get; set; } = new List<Playlisttrack>();

    public virtual ICollection<Trackscreator> Trackscreators { get; set; } = new List<Trackscreator>();
}
