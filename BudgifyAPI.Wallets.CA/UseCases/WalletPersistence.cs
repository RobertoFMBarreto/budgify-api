using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Entities.Requests;
using BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Wallets.CA.UseCases
{
    public static class WalletPersistence
    {
        public static async Task<CustomHttpResponse> RegisterWallet(WalletEntity walletEntity, Guid userId) {
            WalletsContext context = new WalletsContext();
            try
            {
                await walletEntity.Validate();

                Wallet? wallet = await context.Wallets.Where(x => x.Name == walletEntity.WalletName).FirstOrDefaultAsync();
                if (wallet != null) {
                    return new CustomHttpResponse(){Status = 400, Message="Wallet with same name already exists!"};
                }
                else {
                    await context.Wallets.AddAsync(new Wallet() {
                        IdWallet = Guid.NewGuid(),
                        Name = walletEntity.WalletName!,
                        IdUser = userId,
                        StoreInCloud = walletEntity.StoreInCloud,
                        IdAccount = walletEntity.IdAccount,
                        IdRequisition = walletEntity.IdRequisition,
                        
                    });
                    await context.SaveChangesAsync();
                }

                return new CustomHttpResponse(){Status = 200, Message="wallet created successfully"};
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){Status = 500, Message = "Server Error"};
            }

        }
        public static async Task<CustomHttpResponse> DeleteWallet(WalletEntity walletEntity, Guid userId) {
            WalletsContext context = new WalletsContext();

            try {
                Wallet? wallet = await context.Wallets.Where(x => x.IdWallet == walletEntity.WalletId && x.IdUser == userId).FirstOrDefaultAsync();
                if (wallet == null) return new CustomHttpResponse(){Status = 400, Message="Wallet not found"};

                    context.Wallets.Remove(wallet);
                    await context.SaveChangesAsync();
                    return new CustomHttpResponse(){Status = 200, Message="Wallet deleted successfully"};
                
            }

            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){Status = 500, Message = "Server Error"};
            }
        }

        public static async Task<CustomHttpResponse> GetWallet(Guid uid) {
            WalletsContext context = new WalletsContext();

            try {
                List<Wallet>? wallet = await context.Wallets.Where(x => x.IdUser == uid).ToListAsync();

                return new CustomHttpResponse(){Status = 200, Data = wallet};
                    
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){Status = 500, Message = "Server Error"};
            }
        }

        public static async Task<CustomHttpResponse> EditWallet(WalletEntity entity, Guid idUser) {
            WalletsContext context = new WalletsContext();

            try {
                await entity.Validate();

                Wallet? wallet = await context.Wallets.Where(x => x.IdWallet == entity.WalletId && x.IdUser == idUser).FirstOrDefaultAsync();
                if (wallet == null) return new CustomHttpResponse() { Status = 400, Message = "Wallet not found" };
                wallet.Name = entity.WalletName;
                wallet.AgreementDays = entity.AgreementDays;
                wallet.IdRequisition = entity.IdRequisition;
                wallet.IdAccount = entity.IdAccount;
                wallet.StoreInCloud = entity.StoreInCloud;
                context.Wallets.Update(wallet);
                await context.SaveChangesAsync();
                return new CustomHttpResponse(){Status = 200, Data = wallet};
           
        }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){Status = 500, Message = "Server Error"};
            }

        }
    }
}