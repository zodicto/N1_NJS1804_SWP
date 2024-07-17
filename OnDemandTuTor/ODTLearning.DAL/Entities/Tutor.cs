using ODTLearning.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Tutor
{
    public string Id { get; set; } = null!;

    public string? SpecializedSkills { get; set; }

    public string? Introduction { get; set; }

    public int? Experience { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public string? IdAccount { get; set; }

    public virtual ClassRequest? ClassRequest { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<EducationalQualification> EducationalQualifications { get; set; } = new List<EducationalQualification>();

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual ICollection<RequestLearning> RequestLearnings { get; set; } = new List<RequestLearning>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}
