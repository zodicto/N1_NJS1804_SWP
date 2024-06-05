using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Service
{
    public string IdService { get; set; } = null!;

    public float? Price { get; set; }

    public string? Status { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Video { get; set; }

    public string TutoridTutor { get; set; } = null!;

    public string IdTypeOfService { get; set; } = null!;

    public virtual TypeOfService IdTypeOfServiceNavigation { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Tutor TutoridTutorNavigation { get; set; } = null!;
}
