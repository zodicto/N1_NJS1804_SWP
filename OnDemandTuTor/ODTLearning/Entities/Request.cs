using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Request
{
    public string Id { get; set; } = null!;

    public decimal? Price { get; set; }

    public string? Title { get; set; }

    public string? LearningMethod { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? IdAccount { get; set; }

    public string? IdClass { get; set; }

    public string? IdSubject { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Class? IdClassNavigation { get; set; }

    public virtual Subject? IdSubjectNavigation { get; set; }

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual ICollection<RequestLearning> RequestLearnings { get; set; } = new List<RequestLearning>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
