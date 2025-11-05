using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class Track
{
    public int Id { get; set; }

    public string TrackName { get; set; } = null!;

    public TimeOnly Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int Bitrate { get; set; }

    public string FilePath { get; set; } = null!;

    public decimal Rating { get; set; }

    public int PlayCount { get; set; }

    public int AlbumId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
