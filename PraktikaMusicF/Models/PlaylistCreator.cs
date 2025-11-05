using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class PlaylistCreator
{
    public int CrPlId { get; set; }

    public int? UsrId { get; set; }

    public int? PlId { get; set; }

    public virtual Playlist? Pl { get; set; }

    public virtual User? Usr { get; set; }
}
