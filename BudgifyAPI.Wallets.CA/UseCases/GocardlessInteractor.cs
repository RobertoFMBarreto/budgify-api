using BudgifyAPI.Wallets.CA.Entities;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessInteractor
{
    public static async Task<CustomHTTPResponse> GetBankTransactions(Func<string,Task<CustomHTTPResponse>> getBankTransactionsPersistence, string bankAccountId)
    {
        
       return await getBankTransactionsPersistence(bankAccountId);
    }
}