using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class TrackGenre
{
    public int? Track { get; set; }

    public int? Tgenre { get; set; }

    public virtual Genre? TgenreNavigation { get; set; }

    public virtual Track? TrackNavigation { get; set; }
}
