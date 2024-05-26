using System;
using System.Collections.Generic;

namespace OrchidBusinessObject;

public partial class Schedule
{
    public string IdSchedule { get; set; } = null!;

    public DateOnly? Date { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public string? IdTutor { get; set; }

    public string? IdService { get; set; }

    public virtual Service? IdServiceNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
