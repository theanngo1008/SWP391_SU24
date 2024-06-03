using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public string? Message1 { get; set; }

    public byte[]? Image { get; set; }

    public DateOnly? ExtendDate { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
