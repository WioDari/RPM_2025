using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Artistalbum
{
    public int Id { get; set; }

    public int Album { get; set; }

    public int Creator { get; set; }

    public virtual Album AlbumNavigation { get; set; } = null!;

    public virtual Artist CreatorNavigation { get; set; } = null!;
}
