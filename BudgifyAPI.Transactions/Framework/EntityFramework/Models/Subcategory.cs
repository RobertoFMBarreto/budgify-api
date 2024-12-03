using System;
using System.Collections.Generic;

namespace BudgifyAPI.Transactions.Framework.EntityFramework.Models;

public partial class Subcategory
{
    public Guid IdSubcategory { get; set; }

    public Guid IdCategory { get; set; }

    public string Name { get; set; } = null!;

    public Guid? IdUser { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Reocurring> Reocurrings { get; set; } = new List<Reocurring>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
