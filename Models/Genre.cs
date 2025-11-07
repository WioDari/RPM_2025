using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreTittle { get; set; } = null!;

    public virtual ICollection<GenersInTrack> GenersInTracks { get; set; } = new List<GenersInTrack>();

    public virtual ICollection<GenresInAlbum> GenresInAlbums { get; set; } = new List<GenresInAlbum>();

    public virtual ICollection<GenresInArtist> GenresInArtists { get; set; } = new List<GenresInArtist>();
}
