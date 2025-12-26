using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Albumgenre
{
    public int Id { get; set; }

    public int Album { get; set; }

    public int Genre { get; set; }

    public virtual Album AlbumNavigation { get; set; } = null!;

    public virtual Genre GenreNavigation { get; set; } = null!;
}
