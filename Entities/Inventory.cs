﻿using System;
using System.Collections.Generic;

namespace EbayChat.Entities;

public partial class Inventory
{
    public int id { get; set; }

    public int? productId { get; set; }

    public int? quantity { get; set; }

    public DateTime? lastUpdated { get; set; }

    public virtual Product? product { get; set; }
}
