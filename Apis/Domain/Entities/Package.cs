using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Package
{
    public int PackageId { get; set; }

    public string PackageName { get; set; }

    public decimal? Price { get; set; }

    public int? Point { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}