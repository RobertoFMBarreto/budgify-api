using System;
using System.Collections.Generic;

namespace BudgifyAPI.Transactions.Framework.EntityFramework.Models;

public partial class Category
{
    public Guid IdCategory { get; set; }

    public string Name { get; set; } = null!;

    public Guid? IdUser { get; set; }
}
