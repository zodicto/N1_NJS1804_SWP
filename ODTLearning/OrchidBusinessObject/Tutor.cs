using System;
using System.Collections.Generic;

namespace OrchidBusinessObject;

public partial class Tutor
{
    public string IdTutor { get; set; } = null!;

    public string? SpecializedSkills { get; set; }

    public int? Experience { get; set; }

    public string? IdAccount { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<EducationalQualification> EducationalQualifications { get; set; } = new List<EducationalQualification>();

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual ICollection<ResquestTutor> ResquestTutors { get; set; } = new List<ResquestTutor>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<TutorField> TutorFields { get; set; } = new List<TutorField>();
}
