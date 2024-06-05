using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTLearning.Entities;

public partial class TutorField
{
    public string IdTutorFileld { get; set; } = null!;

    public string IdTutor { get; set; } = null!;

    public string IdField { get; set; } = null!;
    [JsonIgnore]
    public virtual Field IdFieldNavigation { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;
}
