using System;
using System.Collections.Generic;

namespace Spotify.Models;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public string SubscriptionName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
