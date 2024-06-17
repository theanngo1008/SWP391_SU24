using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Account
{
    public int AccId { get; set; }

    public string AccName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? NumberPhone { get; set; }

    public decimal? Deposit { get; set; }

    public string? Address { get; set; }

    public string? Role { get; set; }

    public int? Status { get; set; }

    public virtual LoyaltyCard? LoyaltyCard { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
