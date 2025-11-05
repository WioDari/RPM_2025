using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public TimeOnly Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int Bitrate { get; set; }

    public string FilePath { get; set; } = null!;

    public decimal Rating { get; set; }

    public int Playcount { get; set; }

    public string TrackName { get; set; } = null!;

    public int? AlbumId { get; set; }

    public int ArtistId { get; set; }

    public virtual Album? Album { get; set; }

    public virtual ICollection<AlbumTrack> AlbumTracks { get; set; } = new List<AlbumTrack>();

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<GenreTrack> GenreTracks { get; set; } = new List<GenreTrack>();

    public virtual ICollection<PlayListTrack> PlayListTracks { get; set; } = new List<PlayListTrack>();
}
