using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Service
{
    public string Id { get; set; } = null!;

    public float? Price { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Video { get; set; }

    public string IdTutor { get; set; } = null!;

    public string IdLearningModels { get; set; } = null!;

    public virtual LearningModel IdLearningModelsNavigation { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
