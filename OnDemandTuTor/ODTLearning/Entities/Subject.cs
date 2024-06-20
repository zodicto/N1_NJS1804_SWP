using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Subject
{
    public string Id { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}
