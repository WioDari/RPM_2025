using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public string? TrackName { get; set; }

    public int? AlbId { get; set; }

    public DateOnly? TrackRelease { get; set; }

    public int? TrackBitrate { get; set; }

    public string? TrackPath { get; set; }

    public double? TrackRating { get; set; }

    public long? TrackCount { get; set; }

    public long? TrackDuration { get; set; }

    public virtual Album? Alb { get; set; }

    public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();

    public virtual ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();

    public virtual ICollection<TrackList> TrackLists { get; set; } = new List<TrackList>();

    public virtual ICollection<TracksGenre> TracksGenres { get; set; } = new List<TracksGenre>();
}
