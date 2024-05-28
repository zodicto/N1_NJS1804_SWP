using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Post
{
    public string IdPost { get; set; } = null!;

    public string? Price { get; set; }

    public string? Titile { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public string? IdAccount { get; set; }

    public string? IdTypeOfService { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual TypeOfService? IdTypeOfServiceNavigation { get; set; }

    public virtual ICollection<ResquestTutor> ResquestTutors { get; set; } = new List<ResquestTutor>();
}
