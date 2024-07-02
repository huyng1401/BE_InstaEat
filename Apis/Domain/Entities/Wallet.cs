using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Wallet
{
    public int WalletId { get; set; }

    public int? UserId { get; set; }

    public decimal? TotalPoint { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User User { get; set; }
}