using System;
using System.Collections.Generic;

namespace cursach.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public int? ItemTypeId { get; set; }

    public decimal? Price { get; set; }

    public int? AvailableId { get; set; }

    public int? ReservedId { get; set; }

    public int? SoldId { get; set; }

    public string? Status { get; set; }

    public virtual Available? Available { get; set; }

    public virtual ItemType? ItemType { get; set; }

    public virtual Reserved? Reserved { get; set; }

    public virtual Sold? Sold { get; set; }
}
