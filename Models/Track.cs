using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class Track
{
    public int TracksId { get; set; }

    public string TrackName { get; set; } = null!;

    public string? AlbumInfo { get; set; }

    public TimeOnly Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int Bitrate { get; set; }

    public string FilePath { get; set; } = null!;

    public string AlbumCoverPath { get; set; } = null!;

    public string AlbumCoverBinary { get; set; } = null!;

    public decimal Rating { get; set; }

    public int PlayCount { get; set; }

    public virtual ICollection<AlbumTrack> AlbumTracks { get; set; } = new List<AlbumTrack>();

    public virtual ICollection<ArtistTrack> ArtistTracks { get; set; } = new List<ArtistTrack>();

    public virtual ICollection<TracksGenre> TracksGenres { get; set; } = new List<TracksGenre>();

    public virtual ICollection<TracksPlaylist> TracksPlaylists { get; set; } = new List<TracksPlaylist>();
}
