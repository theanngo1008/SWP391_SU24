using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class JewelryMetal
{
    public int? JewelryId { get; set; }

    public int? MetalPriceId { get; set; }

    public decimal? Weight { get; set; }

    public virtual Jewelry? Jewelry { get; set; }

    public virtual SpotMetalPrice? MetalPrice { get; set; }
}
