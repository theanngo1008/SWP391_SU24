using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class SpotMetalPrice
{
    public int MetalPriceId { get; set; }

    public string MetalType { get; set; } = null!;

    public decimal? SpotPrice { get; set; }

    public DateTime? DateRecorded { get; set; }
}
