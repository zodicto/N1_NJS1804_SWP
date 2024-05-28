using System;
using System.Collections.Generic;

namespace OTDLeaningTest.Entities;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public string? Token { get; set; }

    public string? JwtId { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public DateTime? IssuedAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public string IdAccount { get; set; } = null!;

    public virtual Account IdAccountNavigation { get; set; } = null!;
}
