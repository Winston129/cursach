using System;
using System.Collections.Generic;

namespace cursach.Models;

public partial class Available
{
    public int AvailableId { get; set; }

    public DateOnly? DateListed { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
