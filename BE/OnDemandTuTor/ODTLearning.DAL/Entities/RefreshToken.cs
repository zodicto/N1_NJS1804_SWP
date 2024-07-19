using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class RefreshToken
{
    public string Id { get; set; } = null!;

    public string? Token { get; set; }

    public string? JwtId { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public DateTime? IssuedAt { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }
}
