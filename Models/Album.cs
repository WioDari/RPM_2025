using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumTitle { get; set; } = null!;

    public int ReleaseYear { get; set; }

    public string CoverPath { get; set; } = null!;

    public int TotalDuration { get; set; }

    public int? ArtistId { get; set; }

    public virtual ICollection<AlbumGenre> AlbumGenres { get; set; } = new List<AlbumGenre>();

    public virtual ICollection<AlbumTrack> AlbumTracks { get; set; } = new List<AlbumTrack>();

    public virtual Artist? Artist { get; set; }
}
