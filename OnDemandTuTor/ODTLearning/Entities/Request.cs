using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Request
{
    public string IdPost { get; set; } = null!;

    public double? Price { get; set; }

    public string? Titile { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string IdStudent { get; set; } = null!;

    public string IdTypeOfService { get; set; } = null!;

    public virtual Student IdStudentNavigation { get; set; } = null!;

    public virtual TypeOfService IdTypeOfServiceNavigation { get; set; } = null!;

    public virtual ICollection<ResquestLearning> ResquestLearnings { get; set; } = new List<ResquestLearning>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
