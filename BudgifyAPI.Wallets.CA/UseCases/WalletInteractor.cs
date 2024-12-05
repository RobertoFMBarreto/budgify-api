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
            };
            return await walletPersistence( wallet );
        }
    
        public static async Task<CustomHTTPResponse> DeleteWallet(Func<WalletEntity, Task<CustomHTTPResponse>> walletPersistence, Guid userID, Guid walletID ) {
            var wallet = new WalletEntity() {
                UserId = userID,
                WalletId = walletID,
            };
            return await walletPersistence( wallet );
        }
    
            public static async Task<CustomHTTPResponse> GetWallet(Func<WalletEntity, Task<CustomHTTPResponse>> walletPersistence, Guid userID, Guid walletID ) {
            var wallet = new WalletEntity() {
                UserId = userID,
                WalletId = walletID,
            };
            return await walletPersistence( wallet );
        }
    
    }
}