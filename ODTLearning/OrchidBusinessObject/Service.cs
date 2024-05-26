using System;
using System.Collections.Generic;

namespace OrchidBusinessObject;

public partial class Service
{
    public string IdService { get; set; } = null!;

    public float? Price { get; set; }

    public string? Status { get; set; }

    public string? IdTypeOfService { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? IdFeedback { get; set; }

    public virtual Feedback? IdFeedbackNavigation { get; set; }

    public virtual TypeOfService? IdTypeOfServiceNavigation { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
