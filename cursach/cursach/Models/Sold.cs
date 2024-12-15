using System;
using System.Collections.Generic;

namespace cursach.Models;

public partial class Sold
{
    public int SoldId { get; set; }

    public int ClientId { get; set; }

    public DateOnly SaleDate { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
