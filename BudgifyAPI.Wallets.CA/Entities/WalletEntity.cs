using BudgifyAPI.Wallets.CA.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgifyAPI.Wallets.CA.Entities
{
    public class WalletEntity
    {
        public Guid UserId {get; set; }

        public Guid WalletId {get; set; }

        public string? WalletName { get; set; } 

        public string? Requisition {get; set;}

        public string? agreementDays { get; set; }

        public float totalValue { get; set; }
        public async Task<bool> Validate() {
            if (string.IsNullOrEmpty(WalletName) || WalletName.Contains(";") || WalletName.Contains(")")) {
                throw new ArgumentException("Invalid name for wallet");
            }
            return true;
        } 
    }
}