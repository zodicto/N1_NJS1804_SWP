using ODTLearning.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Request
{
    public string Id { get; set; } = null!;

    public float? Price { get; set; }

    public string? Title { get; set; }

    public string? LearningMethod { get; set; }

    public string? Description { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public DateTime CreateDate { get; set; }

    public string? TimeTable { get; set; }

    public int? TotalSession { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public string? IdAccount { get; set; }

    public string? IdClass { get; set; }

    public string? IdSubject { get; set; }

    public virtual ICollection<ClassRequest> ClassRequests { get; set; } = new List<ClassRequest>();

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Class? IdClassNavigation { get; set; }

    public virtual Subject? IdSubjectNavigation { get; set; }

    public virtual ICollection<RequestLearning> RequestLearnings { get; set; } = new List<RequestLearning>();
}
