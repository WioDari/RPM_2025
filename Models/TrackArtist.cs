using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class TrackArtist
{
    public int? RelatedTrack { get; set; }

    public int? RelatedArtist { get; set; }

    public virtual Artist? RelatedArtistNavigation { get; set; }

    public virtual Track? RelatedTrackNavigation { get; set; }
}
