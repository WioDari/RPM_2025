using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string Fio { get; set; } = null!;

    public string? UserEmail { get; set; }

    public int? Role { get; set; }

    public int? SubscriptionType { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public DateOnly LastLogin { get; set; }

    public virtual ICollection<PlayList> PlayLists { get; set; } = new List<PlayList>();

    public virtual UserRole? RoleNavigation { get; set; }

    public virtual Subscription? SubscriptionTypeNavigation { get; set; }
}
