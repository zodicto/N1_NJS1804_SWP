using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Transaction
{
    public string Id { get; set; } = null!;

    public float? Amount { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Status { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }
}
