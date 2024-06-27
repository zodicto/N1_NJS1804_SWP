using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Schedule
{
    public string Id { get; set; } = null!;

    public DateOnly? Date { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public string? IdService { get; set; }

    public string? IdRequest { get; set; }

    public virtual Request? IdRequestNavigation { get; set; }

    public virtual Service? IdServiceNavigation { get; set; }

    public virtual ICollection<Rent1> Rent1s { get; set; } = new List<Rent1>();
}
