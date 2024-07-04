using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public string RestaurantName { get; set; }

    public string Address { get; set; }

    public TimeSpan? OpenTime { get; set; }

    public TimeSpan? CloseTime { get; set; }

    public int? UserId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; }
}