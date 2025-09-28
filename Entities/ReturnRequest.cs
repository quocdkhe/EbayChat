﻿using System;
using System.Collections.Generic;

namespace EbayChat.Entities;

public partial class ReturnRequest
{
    public int id { get; set; }

    public int? orderId { get; set; }

    public int? userId { get; set; }

    public string? reason { get; set; }

    public string? status { get; set; }

    public DateTime? createdAt { get; set; }

    public virtual OrderTable? order { get; set; }

    public virtual User? user { get; set; }
}
