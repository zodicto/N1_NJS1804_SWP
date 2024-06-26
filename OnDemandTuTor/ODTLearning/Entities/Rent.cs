using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Rent
{
    public string Id { get; set; } = null!;

    public decimal? Price { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? IdSubject { get; set; }

    public string? IdRequest { get; set; }

    public string? IdTutor { get; set; }

    public string? IdAccount { get; set; }

    public virtual Request? IdRequestNavigation { get; set; }
}
