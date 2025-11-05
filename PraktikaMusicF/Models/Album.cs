using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class Album
{
    public int AlbId { get; set; }

    public string? AlbName { get; set; }

    public int? ArtistId { get; set; }

    public string? AlbImage { get; set; }

    public DateOnly? AlbRelease { get; set; }

    public virtual ICollection<AlbumsGenre> AlbumsGenres { get; set; } = new List<AlbumsGenre>();

    public virtual Artist? Artist { get; set; }

    public virtual ICollection<TrackList> TrackLists { get; set; } = new List<TrackList>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
