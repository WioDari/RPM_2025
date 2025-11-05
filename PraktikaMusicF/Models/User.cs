using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class User
{
    public int UsrId { get; set; }

    public string? UsrFName { get; set; }

    public string? UsrEmail { get; set; }

    public int? RoleId { get; set; }

    public int? SubId { get; set; }

    public DateOnly? UsrRegDate { get; set; }

    public DateOnly? UsrLastEntry { get; set; }

    public int? UsrPlaylistId { get; set; }

    public string? UsrPass { get; set; }

    public virtual ICollection<PlaylistCreator> PlaylistCreators { get; set; } = new List<PlaylistCreator>();

    public virtual Role? Role { get; set; }

    public virtual Subscription? Sub { get; set; }

    public virtual Playlist? UsrPlaylist { get; set; }
}
