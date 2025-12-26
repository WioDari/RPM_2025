using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Artistgenre
{
    public int Id { get; set; }

    public int Creator { get; set; }

    public int Genre { get; set; }

    public virtual Artist CreatorNavigation { get; set; } = null!;

    public virtual Genre GenreNavigation { get; set; } = null!;
}
