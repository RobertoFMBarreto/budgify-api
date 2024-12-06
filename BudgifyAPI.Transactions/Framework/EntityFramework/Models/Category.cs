using System;
using System.Collections.Generic;

namespace BudgifyAPI.Transactions.Framework.EntityFramework.Models;

public partial class Category
{
    public Guid IdCategory { get; set; }

    public string Name { get; set; } = null!;

    public Guid? IdUser { get; set; }

    public virtual ICollection<Reocurring> Reocurrings { get; set; } = new List<Reocurring>();

    public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
