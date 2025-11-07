using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public string SubscriptionTittle { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
