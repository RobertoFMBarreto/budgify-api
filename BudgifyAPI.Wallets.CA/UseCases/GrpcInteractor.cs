using BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;

namespace BudgifyAPI.Wallets.CA.UseCases;

public class GrpcInteractor
{
    public static async Task<List<string>> GetUserWalletsInteractor(Func<Guid,Task<List<string>>> getUserWalletsPersistence,Guid uid)
    {
        return await getUserWalletsPersistence(uid);
    }
    public static async Task<Wallet?> GetWalletsById(Func<Guid, Guid,Task<Wallet?>> getWalletByIdPersistence,Guid walletId,Guid uid)
    {
        return await getWalletByIdPersistence(walletId,uid);
    }
}