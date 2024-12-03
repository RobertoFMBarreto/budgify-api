using System;
using System.Collections.Generic;

namespace BudgifyAPI.Transactions.Framework.EntityFramework.Models;

public partial class TransactionGroup
{
    public Guid IdTransactionGroup { get; set; }

    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public float PlannedAmount { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
