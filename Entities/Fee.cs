using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Fee
{
    public int FeeId { get; set; }

    public string? FeeType { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? DateAdded { get; set; }
}
