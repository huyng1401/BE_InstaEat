using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? RestaurantId { get; set; }

    public int? UserId { get; set; }

    public int? ReviewId { get; set; }

    public int? Amount { get; set; }

    public DateTime? Created { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Restaurant Restaurant { get; set; }

    public virtual Review Review { get; set; }

    public virtual User User { get; set; }
}