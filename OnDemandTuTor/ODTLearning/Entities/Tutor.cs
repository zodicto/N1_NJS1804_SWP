using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTLearning.Entities;

public partial class Tutor
{
    public string IdTutor { get; set; } = null!;

    public string? SpecializedSkills { get; set; }

    public int? Experience { get; set; }

    public string? Status { get; set; }

    public string IdAccount { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<EducationalQualification> EducationalQualifications { get; set; } = new List<EducationalQualification>();

    public virtual Acount IdAccountNavigation { get; set; } = null!;

    public virtual ICollection<ResquestLearning> ResquestLearnings { get; set; } = new List<ResquestLearning>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<TutorField> TutorFields { get; set; } = new List<TutorField>();


  

}
