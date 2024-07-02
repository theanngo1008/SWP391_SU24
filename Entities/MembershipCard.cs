using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class MembershipCard
{
    public int CardId { get; set; }

    public string CardNumber { get; set; } = null!;

    public DateTime? IssueDate { get; set; }

    public string? MembershipLevel { get; set; }

    public decimal? Points { get; set; }

    public bool? Status { get; set; }

    public int? AccId { get; set; }

    public virtual Account? Acc { get; set; }
}
