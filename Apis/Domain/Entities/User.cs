using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public int? RoleId { get; set; }

    public string Phone { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}