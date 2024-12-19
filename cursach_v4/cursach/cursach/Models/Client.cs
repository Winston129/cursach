using System;
using System.Collections.Generic;

namespace cursach.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? MiddleName { get; set; }

    public string? PassportData { get; set; }

    public virtual ICollection<Reserved> Reserveds { get; set; } = new List<Reserved>();

    public virtual ICollection<Sold> Solds { get; set; } = new List<Sold>();
}
