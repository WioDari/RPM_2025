using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public string? Subscription1 { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
