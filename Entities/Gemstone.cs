using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Gemstone
{
    public int GemstoneId { get; set; }

    public string? GemstoneName { get; set; }

    public string? Image { get; set; }

    public decimal? GemstoneCost { get; set; }

    public bool? Status { get; set; }
}
