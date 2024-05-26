using System;
using System.Collections.Generic;

namespace OrchidBusinessObject;

public partial class Field
{
    public string IdField { get; set; } = null!;

    public string? FieldName { get; set; }

    public virtual ICollection<TutorField> TutorFields { get; set; } = new List<TutorField>();
}
