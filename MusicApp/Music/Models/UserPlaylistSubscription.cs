using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class UserPlaylistSubscription
{
    public int UserId { get; set; }

    public int PlaylistId { get; set; }

    public DateTime SubscriptionDate { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
