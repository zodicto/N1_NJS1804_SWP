using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Acount
{
    public string IdAccount { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gmail { get; set; }

    public string? Birthdate { get; set; }

    public string? Gender { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
