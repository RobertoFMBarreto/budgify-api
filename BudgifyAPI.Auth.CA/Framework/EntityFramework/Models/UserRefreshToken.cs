using System;
using System.Collections.Generic;

namespace BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;

public partial class UserRefreshToken
{
    public Guid IdToken { get; set; }

    public Guid IdUser { get; set; }

    public string Token { get; set; } = null!;

    public string Device { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime LastUsage { get; set; }
}
