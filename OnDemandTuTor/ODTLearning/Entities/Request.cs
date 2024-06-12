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

    public string IdAccount { get; set; } = null!;

    public string IdLearningModels { get; set; } = null!;

    public string IdSubject { get; set; } = null!;

    public virtual Account IdAccountNavigation { get; set; } = null!;

    public virtual LearningModel IdLearningModelsNavigation { get; set; } = null!;

    public virtual Subject IdSubjectNavigation { get; set; } = null!;

    public virtual ICollection<RequestLearning> RequestLearnings { get; set; } = new List<RequestLearning>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
