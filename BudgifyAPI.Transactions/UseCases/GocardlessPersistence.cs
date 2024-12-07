using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessPersistence
{
    private static async Task<Dictionary<string, object>> GetAccessTokenPersistence()
    {
        
        try
        {
            var environmentName =
                Environment.GetEnvironmentVariable(
                    "ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

            Byte[]secret_key_bytes = Convert.FromBase64String(config["gocardless:secret-key"]);
            string secret_key= Encoding.UTF8.GetString(secret_key_bytes);
            
            Byte[]secret_id_bytes = Convert.FromBase64String(config["gocardless:secret-id"]);
            string secret_id= Encoding.UTF8.GetString(secret_id_bytes);
            
            string url = $"https://bankaccountdata.gocardless.com/api/v2/token/new/";
            HttpClient client = new HttpClient();
            var body = new
            {
                secret_id = secret_id,
                secret_key = secret_key,
            };
            string json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var access =
                JsonSerializer.Deserialize<Dictionary<string, object>>(response.Content.ReadAsStringAsync().Result);
            
            return access;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static async Task<CustomHttpResponse> GetTransactionsPersistence(Guid walletId, string idAccount, Guid userId)
    {
        try
        {
            IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
            string[] walletsArray = wallets.ToArray();
            Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
            if(!guids.Contains(walletId))
            {
                return new CustomHttpResponse() { status = 400, message = "Invalid wallet" };
            }
            
            Dictionary<string, object> accessResponse = await GetAccessTokenPersistence();
            string url = $"https://bankaccountdata.gocardless.com/api/v2/accounts/{idAccount}/transactions/";
            HttpClient client = new HttpClient();
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessResponse["access"].ToString());
            var response = await client.SendAsync(requestMessage);
            Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> transactions = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>>(response.Content.ReadAsStringAsync().Result);
            if (transactions.ContainsKey("status_code"))
            {
                if (transactions["status_code"].ToString() == "429")
                {
                    return new CustomHttpResponse(){status =int.Parse(transactions["status_code"].ToString()), message = transactions["detail"].ToString() };
                }
            }
            Task.Run(()=>StoreGoCardlessTransacitons(transactions,walletId,userId));
            
            return new CustomHttpResponse(){status =(int)response.StatusCode, Data =transactions };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private static async Task StoreGoCardlessTransacitons(Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> transactions, Guid walletId, Guid userId)
    {
        try
        { 
            
            foreach (Dictionary<string, object> transaction in transactions["transactions"]["booked"])
            {
                var valueAmount = JsonSerializer.Deserialize<Dictionary<string, string>>(transaction["transactionAmount"].ToString());
                Console.WriteLine(DateTime.Parse(transaction["valueDate"].ToString()));
                CreateTransaction createTransactionEntity = new CreateTransaction()
                {
                    IdWallet = walletId,
                    Date = DateTime.Parse(transaction["valueDate"].ToString()).ToUniversalTime(),
                    Amount = Single.Parse(valueAmount["amount"],CultureInfo.InvariantCulture),
                    Description = transaction["remittanceInformationUnstructured"].ToString(),
                    IsPlanned = false,
                };
                await TransactionsInteractorEF.AddTransaction(TransactionsPersistence.AddTransactionPersistence,createTransactionEntity, userId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}