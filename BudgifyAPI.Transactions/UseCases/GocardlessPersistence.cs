using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;
using Getwalletsgrpcservice;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessPersistence
{
    private static async Task<Dictionary<string, object>> GetAccessTokenPersistence()
    {
        try
        {
            Byte[] secretKeyBytes =
                Convert.FromBase64String(Environment.GetEnvironmentVariable(
                    "gocardless__secret_key"));

            string secretKey = Encoding.UTF8.GetString(secretKeyBytes);

            Byte[] secretIdBytes =
                Convert.FromBase64String(Environment.GetEnvironmentVariable(
                    "gocardless__secret_id"));
            string secretId = Encoding.UTF8.GetString(secretIdBytes);

            string url = $"https://bankaccountdata.gocardless.com/api/v2/token/new/";
            HttpClient client = new HttpClient();
            var body = new
            {
                secret_id = secretId,
                secret_key = secretKey,
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

    public static async Task<CustomHttpResponse> GetTransactionsPersistence(Guid walletId, Guid userId)
    {
        try
        {
            IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
            string[] walletsArray = wallets.ToArray();
            Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
            if (!guids.Contains(walletId))
            {
                return new CustomHttpResponse() { Status = 400, Message = "Invalid wallet" };
            }

            GetWalletByIdResponse wallet = await WalletsServiceClient.GetWalletById(walletId, userId);
            if (wallet.AccountId == null)
            {
                return new CustomHttpResponse() { Status = 400, Message = "Invalid wallet" };
            }

            Dictionary<string, object> accessResponse = await GetAccessTokenPersistence();
            string url = $"https://bankaccountdata.gocardless.com/api/v2/accounts/{wallet.AccountId}/transactions/";
            HttpClient client = new HttpClient();
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessResponse["access"].ToString());
            var response = await client.SendAsync(requestMessage);
            Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> transactions =
                JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>>(
                    response.Content.ReadAsStringAsync().Result);
            if (transactions.ContainsKey("status_code"))
            {
                if (transactions["status_code"].ToString() == "429")
                {
                    return new CustomHttpResponse()
                    {
                        Status = int.Parse(transactions["status_code"].ToString()),
                        Message = transactions["detail"].ToString()
                    };
                }
            }

            if (wallet.StoreInCloud)
            {
                Task.Run(() => StoreGoCardlessTransacitons(transactions, walletId, userId));
            }


            return new CustomHttpResponse() { Status = (int)response.StatusCode, Data = transactions };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static async Task StoreGoCardlessTransacitons(
        Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> transactions, Guid walletId,
        Guid userId)
    {
        try
        {
            foreach (Dictionary<string, object> transaction in transactions["transactions"]["booked"])
            {
                var valueAmount =
                    JsonSerializer.Deserialize<Dictionary<string, string>>(transaction["transactionAmount"].ToString());
                Console.WriteLine(DateTime.Parse(transaction["valueDate"].ToString()));
                CreateTransaction createTransactionEntity = new CreateTransaction()
                {
                    IdWallet = walletId,
                    Date = DateTime.Parse(transaction["valueDate"].ToString()).ToUniversalTime(),
                    Amount = Single.Parse(valueAmount["amount"], CultureInfo.InvariantCulture),
                    Description = transaction["remittanceInformationUnstructured"].ToString(),
                    IsPlanned = false,
                };
                await TransactionsInteractorEf.AddTransaction(TransactionsPersistence.AddTransactionPersistence,
                    createTransactionEntity, userId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}