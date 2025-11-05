using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? GenresName { get; set; }

    public virtual ICollection<AlbumsGenre> AlbumsGenres { get; set; } = new List<AlbumsGenre>();

    public virtual ICollection<ArtistGenre> ArtistGenres { get; set; } = new List<ArtistGenre>();

    public virtual ICollection<TracksGenre> TracksGenres { get; set; } = new List<TracksGenre>();
}
