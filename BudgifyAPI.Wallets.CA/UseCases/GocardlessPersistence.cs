using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessPersistence
{
    public static async Task<CustomHttpResponse> GetBankTransactionsPersistence(string bankAccountId)
    {
        
        try
        {
            string url = $"https://www.gocardless.com/api/v2/accounts/{bankAccountId}/transactions";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(response.StatusCode);
            return new CustomHttpResponse()
            {
                status = (int)response.StatusCode,
                Data = response.Content.ReadAsStringAsync().Result,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}