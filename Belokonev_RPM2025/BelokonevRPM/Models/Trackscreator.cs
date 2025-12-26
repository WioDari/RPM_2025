using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Trackscreator
{
    public int Id { get; set; }

    public int TrackId { get; set; }

    public int Creator { get; set; }

    public virtual Artist CreatorNavigation { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
