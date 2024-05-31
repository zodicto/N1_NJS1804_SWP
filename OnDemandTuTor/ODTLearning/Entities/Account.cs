using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Account
{
    public string IdAccount { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FisrtName { get; set; }

    public string? LastName { get; set; }

    public string? Gmail { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? Gender { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
