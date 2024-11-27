using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BudgifyAPI.Wallets.CA.Entities;

namespace BudgifyAPI.Wallets.CA.UseCases
{
    public static class WalletInteractorEF
    {

        public static async Task<CustomHTTPResponse> RegisterWallet(Func<WalletEntity, Task<CustomHTTPResponse>> walletPersistence, Guid userID, string walletName ) {
            var wallet = new WalletEntity() {
                WalletName = walletName,
                UserId = userID,
                WalletId = new Guid(),

            };
            return await walletPersistence( wallet );
        }
    
        
    }
}