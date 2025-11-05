using System;
using System.Collections.Generic;

namespace Spotify.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public string TrackName { get; set; } = null!;

    public int? AlbumId { get; set; }

    public int ArtistId { get; set; }

    public TimeSpan Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int Bitrate { get; set; }

    public string FilePath { get; set; } = null!;

    public decimal Rating { get; set; }

    public long PlayCount { get; set; }

    public virtual Album? Album { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
