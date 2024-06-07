using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Schedule
{
    public string Id { get; set; } = null!;

    public DateOnly? Date { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public string IdService { get; set; } = null!;

    public string IdRequest { get; set; } = null!;

    public virtual Request IdRequestNavigation { get; set; } = null!;

    public virtual Service IdServiceNavigation { get; set; } = null!;

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
