using BudgifyAPI.Transactions.Entities;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessInteractor
{
    public static async Task<CustomHttpResponse> GetTransactionsInteractor(Func<Guid,Guid,Task<CustomHttpResponse>> getTransactionsPersistence,  Guid idUser, Guid idWallet)
    {
        return await getTransactionsPersistence(idWallet, idUser);
    }
}