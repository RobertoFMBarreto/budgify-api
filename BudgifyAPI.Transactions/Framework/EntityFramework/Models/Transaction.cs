using System;
using System.Collections.Generic;

namespace BudgifyAPI.Transactions.Framework.EntityFramework.Models;

public partial class Transaction
{
    public Guid IdTransaction { get; set; }

    public Guid IdWallet { get; set; }

    public Guid? IdCategory { get; set; }

    public Guid? IdSubcategory { get; set; }

    public Guid? IdTransactionGroup { get; set; }

    public Guid? IdReocurring { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; } = null!;

    public float Amount { get; set; }

    public bool IsPlanned { get; set; }

    public float? Latitude { get; set; }

    public float? Longitue { get; set; }
}
