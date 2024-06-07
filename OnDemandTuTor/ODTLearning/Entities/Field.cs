using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Field
{
    public string Id { get; set; } = null!;

    public string FieldName { get; set; } = null!;

    public virtual ICollection<TutorField> TutorFields { get; set; } = new List<TutorField>();
}
