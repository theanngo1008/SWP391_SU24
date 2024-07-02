using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class SpotGoldPrice
{
    public int GoldPriceId { get; set; }

    public string GoldType { get; set; } = null!;

    public decimal? SpotPrice { get; set; }

    public DateTime? DateRecorded { get; set; }
}
