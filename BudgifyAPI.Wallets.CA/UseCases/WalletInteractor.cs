using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Wallets.CA.Entities;

namespace BudgifyAPI.Wallets.CA.UseCases
{
    public static class WalletInteractorEF
    {

        public static async Task<CustomHttpResponse> RegisterWallet(Func<WalletEntity, Task<CustomHttpResponse>> walletPersistence, Guid userID, string walletName ) {
            var wallet = new WalletEntity() {
                WalletName = walletName,
                UserId = userID,
            };
            return await walletPersistence( wallet );
        }
    
        public static async Task<CustomHttpResponse> DeleteWallet(Func<WalletEntity, Task<CustomHttpResponse>> walletPersistence, Guid userID, Guid walletID ) {
            var wallet = new WalletEntity() {
                UserId = userID,
                WalletId = walletID,
            };
            return await walletPersistence( wallet );
        }
    
        public static async Task<CustomHttpResponse> GetWallet(Func<Guid, Task<CustomHttpResponse>> walletPersistence, Guid userID ) {
            return await walletPersistence( userID );
        }
    
    }
}