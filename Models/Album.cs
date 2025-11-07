using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumTitle { get; set; } = null!;

    public int ReleaseYear { get; set; }

    public string CoverPath { get; set; } = null!;

    public int? ArtistId { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual ICollection<GenresInAlbum> GenresInAlbums { get; set; } = new List<GenresInAlbum>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<TracksInAlbum> TracksInAlbums { get; set; } = new List<TracksInAlbum>();
}
