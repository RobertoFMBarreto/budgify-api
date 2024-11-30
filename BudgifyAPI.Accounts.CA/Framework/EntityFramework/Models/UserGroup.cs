using System;
using System.Collections.Generic;

namespace BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;

public partial class UserGroup
{
    public Guid IdUserGroup { get; set; }

    public string Name { get; set; } = null!;
}
