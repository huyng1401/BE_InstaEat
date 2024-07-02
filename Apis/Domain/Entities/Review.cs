using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? RestaurantId { get; set; }

    public string Content { get; set; }

    public string Image { get; set; }

    public int? Status { get; set; }

    public DateTime? Created { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Restaurant Restaurant { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; }
}