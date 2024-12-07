using BudgifyAPI.Transactions.Entities;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessInteractor
{
    public static async Task<CustomHttpResponse> GetTransactionsInteractor(Func<Guid,string,Guid,Task<CustomHttpResponse>> getTransactionsPersistence, string idAccount, Guid idUser, Guid idWallet)
    {
        return await getTransactionsPersistence(idWallet, idAccount, idUser);
    }
}