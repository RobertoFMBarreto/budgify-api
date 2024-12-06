using BudgifyAPI.Wallets.CA.Entities;
using System;
using System.Collections.Generic;

namespace BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;

public partial class Wallet
{
    public Guid IdWallet { get; set; }

    public Guid IdUser { get; set; }

    public string Name { get; set; } = null!;

    public string? Requisition { get; set; }

    public string? AgreementDays { get; set; }

    public float totalValue { get; set; }   

    public void FromEntity(WalletEntity entity)
    {
        IdWallet = entity.WalletId;
        IdUser = entity.UserId;
        Name = entity.WalletName;
        Requisition = entity.Requisition;
        AgreementDays = entity.agreementDays;
        totalValue = entity.totalValue;
    }
}
