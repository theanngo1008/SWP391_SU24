using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class JewelryGold
{
    public int? JewelryId { get; set; }

    public int? GoldPriceId { get; set; }

    public decimal? Weight { get; set; }

    public virtual SpotGoldPrice? GoldPrice { get; set; }

    public virtual Jewelry? Jewelry { get; set; }
}
