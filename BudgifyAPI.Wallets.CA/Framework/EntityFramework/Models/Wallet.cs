using System;
using System.Collections.Generic;

namespace BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;

public partial class Wallet
{
    public Guid IdWallet { get; set; }

    public Guid IdUser { get; set; }

    public string Name { get; set; } = null!;

    public string? Requisition { get; set; }

    public int? AgreementDays { get; set; }
}
