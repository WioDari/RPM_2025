using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class AlbumGenre
{
    public int AlbumGenresId { get; set; }

    public int AlbumId { get; set; }

    public int GenreId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
