using System;
using System.Collections.Generic;

namespace BudgifyAPI.Auth.Models.DB;

public partial class User
{
    public Guid IdUser { get; set; }

    public Guid? IdUserGroup { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public int Genre { get; set; }

    public bool IsActive { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsManager { get; set; }

    public bool AllowWalletWatch { get; set; }

    public virtual UserGroup? IdUserGroupNavigation { get; set; }
}
