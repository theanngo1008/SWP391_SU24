using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class JewelryGemstone
{
    public int JewelryGemstoneId { get; set; }

    public int? JewelryId { get; set; }

    public int? GemstoneId { get; set; }

    public virtual Gemstone? Gemstone { get; set; }

    public virtual Jewelry? Jewelry { get; set; }
}
