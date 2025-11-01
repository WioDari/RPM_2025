using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public string? SubscriptionName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
