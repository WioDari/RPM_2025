using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public string TrackName { get; set; } = null!;

    public TimeOnly Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int Bitrate { get; set; }

    public string FilePath { get; set; } = null!;

    public string AlbumCoverPath { get; set; } = null!;

    public decimal Rating { get; set; }

    public int PlayCount { get; set; }

    public int? AlbumId { get; set; }

    public virtual Album? Album { get; set; }

    public virtual ICollection<ArtistsInTrack> ArtistsInTracks { get; set; } = new List<ArtistsInTrack>();

    public virtual ICollection<GenersInTrack> GenersInTracks { get; set; } = new List<GenersInTrack>();

    public virtual ICollection<TracksInAlbum> TracksInAlbums { get; set; } = new List<TracksInAlbum>();

    public virtual ICollection<TracksInPlaylist> TracksInPlaylists { get; set; } = new List<TracksInPlaylist>();
}
