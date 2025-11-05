using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class GenresInAlbum
{
    public int GiAlId { get; set; }

    public int GenreId { get; set; }

    public int AlbumId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
