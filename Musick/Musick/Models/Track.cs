using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public string TrackName { get; set; } = null!;

    public int Duration { get; set; }

    public DateOnly RealiseDate { get; set; }

    public int Bitrate { get; set; }

    public string FilePath { get; set; } = null!;

    public decimal Rating { get; set; }

    public int? PlayCount { get; set; }

    public virtual ICollection<AlbumsTreck> AlbumsTrecks { get; set; } = new List<AlbumsTreck>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
