using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class LoyaltyCard
{
    public int CardId { get; set; }

    public string? CardName { get; set; }

    public bool? Status { get; set; }

    public int? AccId { get; set; }

    public virtual Account? Acc { get; set; }
}
