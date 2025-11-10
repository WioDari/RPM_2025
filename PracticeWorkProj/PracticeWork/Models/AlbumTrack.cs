using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class AlbumTrack
{
    public int? Talbum { get; set; }

    public int? Atrack { get; set; }

    public virtual Track? AtrackNavigation { get; set; }

    public virtual Album? TalbumNavigation { get; set; }
}
