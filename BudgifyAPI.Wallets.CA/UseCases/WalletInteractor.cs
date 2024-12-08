using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Entities.Requests;

namespace BudgifyAPI.Wallets.CA.UseCases
{
    public static class WalletInteractorEf
    {

        public static async Task<CustomHttpResponse> RegisterWallet(Func<WalletEntity, Guid, Task<CustomHttpResponse>> walletPersistence, Guid userId, string walletName ) {
            var wallet = new WalletEntity() {
                WalletName = walletName,
            };
            return await walletPersistence( wallet, userId );
        }
    
        public static async Task<CustomHttpResponse> DeleteWallet(Func<WalletEntity, Guid, Task<CustomHttpResponse>> walletPersistence, Guid userId, Guid walletId ) {
            var wallet = new WalletEntity() {
                WalletId = walletId,
            };
            return await walletPersistence( wallet, userId );
        }
    
        public static async Task<CustomHttpResponse> GetWallet(Func<Guid, Task<CustomHttpResponse>> walletPersistence, Guid userId ) {
            return await walletPersistence( userId );
        }

        public static async Task<CustomHttpResponse> EditWallet(Func<WalletEntity,Guid, Task<CustomHttpResponse>> walletPersistence, EditWalletRequest req, Guid idUser)
        {
            var wallet = new WalletEntity()
            {
                WalletId = req.WalletId,
                WalletName = req.WalletName,
                TotalValue = req.Value,
                AgreementDays = req.AgreementDays,
                IdRequisition = req.IdRequisition,
                IdAccount = req.IdAccount,
            };
            return await walletPersistence( wallet, idUser);
        }
    
    }
}