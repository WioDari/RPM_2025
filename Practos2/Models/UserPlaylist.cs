using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class UserPlaylist
{
    public int Userplaylistid { get; set; }

    public int? Userid { get; set; }

    public int? Playlistid { get; set; }

    public virtual User? User { get; set; }
}
