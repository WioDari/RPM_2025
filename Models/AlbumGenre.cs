using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spotify_wpf.Models;

public partial class AlbumGenre
{
   
    public int AlbumGenreId { get; set; }

    public int GenreId { get; set; }

    public int? AlbumId { get; set; }

    public virtual Album? Album { get; set; }

    public virtual Genre Genre { get; set; } = null!;
}
