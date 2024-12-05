namespace BudgifyAPI.Wallets.CA.UseCases;

public class GrpcInteractor
{
    public static async Task<List<string>> GetUserWalletsInteractor(Func<Guid,Task<List<string>>> getUserWalletsPersistence,Guid uid)
    {
        return await getUserWalletsPersistence(uid);
    }
}