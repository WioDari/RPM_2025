using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Subscription
{
    public short Subscriptionid { get; set; }

    public string Subscriptionname { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
