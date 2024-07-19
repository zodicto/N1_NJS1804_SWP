using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Subject
{
    public string Id { get; set; } = null!;

    public string? SubjectName { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}
