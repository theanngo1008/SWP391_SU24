﻿using System;
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

    public string? Image { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? Status { get; set; }

    public virtual MembershipCard? MembershipCard { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
