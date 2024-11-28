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

        public string? WalletName {get; set;}

        public string? Requisition {get; set;}

        
    }
}