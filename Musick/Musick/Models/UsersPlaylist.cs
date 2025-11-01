using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class UsersPlaylist
{
    public int UserId { get; set; }

    public int PlaylistId { get; set; }
}
