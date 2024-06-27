using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Tutor
{
    public string Id { get; set; } = null!;

    public string? SpecializedSkills { get; set; }

    public int? Experience { get; set; }

    public string? Status { get; set; }

    public string? IdAccount { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<EducationalQualification> EducationalQualifications { get; set; } = new List<EducationalQualification>();

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual ICollection<RequestLearning> RequestLearnings { get; set; } = new List<RequestLearning>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}
