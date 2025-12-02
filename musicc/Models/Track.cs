using System;
using System.Collections.Generic;

namespace musicc.Models;

public partial class Track
{
    public int Id { get; set; }

    public string TrackName { get; set; } = null!;

    public int Duration { get; set; }

    public DateOnly Realise { get; set; }

    public int Bitrate { get; set; }

    public string Filepath { get; set; } = null!;

    public decimal Rating { get; set; }

    public int PlayCount { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Gener> Geners { get; set; } = new List<Gener>();

    public virtual ICollection<Playlist> IdPlaylists { get; set; } = new List<Playlist>();
}
