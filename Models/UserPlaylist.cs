using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class UserPlaylist
{
    public int? User { get; set; }

    public int? Uplaylist { get; set; }

    public virtual PlayList? UplaylistNavigation { get; set; }

    public virtual User? UserNavigation { get; set; }
}
