using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int? RestaurantId { get; set; }

    public int? PackageId { get; set; }

    public DateTime? OrderDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Package Package { get; set; }

    public virtual Restaurant Restaurant { get; set; }
}