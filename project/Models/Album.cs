using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumName { get; set; } = null!;

    public string CoverPath { get; set; } = null!;

    public int TotalDuration { get; set; }

    public int ReleaseYear { get; set; }

    public int ArtistId { get; set; }

    public virtual ICollection<AlbumGenre> AlbumGenres { get; set; } = new List<AlbumGenre>();

    public virtual ICollection<AlbumTrack> AlbumTracks { get; set; } = new List<AlbumTrack>();

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
