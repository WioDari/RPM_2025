using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class GenreArtist
{
    public int GenreArtistId { get; set; }

    public int GenreId { get; set; }

    public int ArtistId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
