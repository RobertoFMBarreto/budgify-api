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
                    return new CustomHttpResponse(){status = 400, message="Wallet with same name already exists!"};
                }
                else {
                    await context.Wallets.AddAsync(new Wallet() {
                        IdWallet = Guid.NewGuid(),
                        Name = walletEntity.WalletName!,
                        IdUser = userId
                        
                    });
                    await context.SaveChangesAsync();
                }

                return new CustomHttpResponse(){status = 200, message="wallet created successfully"};
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){status = 500, message = "Server Error"};
            }

        }
        public static async Task<CustomHttpResponse> DeleteWallet(WalletEntity walletEntity, Guid userId) {
            WalletsContext context = new WalletsContext();

            try {
                Wallet? wallet = await context.Wallets.Where(x => x.IdWallet == walletEntity.WalletId && x.IdUser == userId).FirstOrDefaultAsync();
                if (wallet == null) return new CustomHttpResponse(){status = 400, message="Wallet not found"};

                    context.Wallets.Remove(wallet);
                    await context.SaveChangesAsync();
                    return new CustomHttpResponse(){status = 200, message="Wallet deleted successfully"};
                
            }

            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){status = 500, message = "Server Error"};
            }
        }

        public static async Task<CustomHttpResponse> GetWallet(Guid uid) {
            WalletsContext context = new WalletsContext();

            try {
                List<Wallet>? wallet = await context.Wallets.Where(x => x.IdUser == uid).ToListAsync();

                return new CustomHttpResponse(){status = 200, Data = wallet};
                    
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){status = 500, message = "Server Error"};
            }
        }

        public static async Task<CustomHttpResponse> EditWallet(WalletEntity entity, Guid IdUser) {
            WalletsContext context = new WalletsContext();

            try {
                await entity.Validate();

                Wallet? wallet = await context.Wallets.Where(x => x.IdWallet == entity.WalletId && x.IdUser == IdUser).FirstOrDefaultAsync();
                if (wallet == null) return new CustomHttpResponse() { status = 400, message = "Wallet not found" };
                wallet.Name = entity.WalletName;
                wallet.AgreementDays = entity.AgreementDays;
                wallet.IdRequisition = entity.IdRequisition;
                wallet.IdAccount = entity.IdAccount;
                context.Wallets.Update(wallet);
                await context.SaveChangesAsync();
                return new CustomHttpResponse(){status = 200, Data = wallet};
           
        }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHttpResponse(){status = 500, message = "Server Error"};
            }

        }
    }
}