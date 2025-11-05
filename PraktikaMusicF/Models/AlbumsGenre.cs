using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class AlbumsGenre
{
    public int AlbGenresId { get; set; }

    public int? AlbId { get; set; }

    public int? GenreId { get; set; }

    public virtual Album? Alb { get; set; }

    public virtual Genre? Genre { get; set; }
}
