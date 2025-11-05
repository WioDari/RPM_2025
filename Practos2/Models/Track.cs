using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Track
{
    public int Trackid { get; set; }

    public string? Trackname { get; set; }

    public int? Artistid { get; set; }

    public int? Albumid { get; set; }

    public int? Duration { get; set; }

    public DateOnly? Releasedate { get; set; }

    public int? Bitrate { get; set; }

    public string? Filepath { get; set; }

    public decimal? Raiting { get; set; }

    public int? Playcount { get; set; }

    public int? Albumcovarid { get; set; }

    public virtual Album? Album { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual ICollection<GenreTrack> GenreTracks { get; set; } = new List<GenreTrack>();

    public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
}
