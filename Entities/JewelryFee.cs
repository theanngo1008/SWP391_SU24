using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class JewelryFee
{
    public int? JewelryId { get; set; }

    public int? FeeId { get; set; }

    public virtual Fee? Fee { get; set; }

    public virtual Jewelry? Jewelry { get; set; }
}
