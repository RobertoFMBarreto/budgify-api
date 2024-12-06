using BudgifyAPI.Transactions.Entities;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessInteractor
{
    public static async Task<CustomHttpResponse> GetBankTransactions(Func<string,Task<CustomHttpResponse>> getBankTransactionsPersistence, string bankAccountId)
    {
        
       return await getBankTransactionsPersistence(bankAccountId);
    }
}