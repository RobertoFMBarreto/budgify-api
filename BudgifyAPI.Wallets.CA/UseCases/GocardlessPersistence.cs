using System.Text;
using System.Text.Json;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Entities.Responses.GoCardless;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessPersistence
{
    public static async Task<CustomHTTPResponse> GetAccessToken(string bankAccountId)
    {
        
        try
        {
            var environmentName =
                Environment.GetEnvironmentVariable(
                    "ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

            Byte[]secret_key_bytes = Convert.FromBase64String(config["gocardless:secret_key"]);
            string secret_key= Encoding.UTF8.GetString(secret_key_bytes);
            
            Byte[]secret_id_bytes = Convert.FromBase64String(config["gocardless:secret_id"]);
            string secret_id= Encoding.UTF8.GetString(secret_id_bytes);
            
            string url = $"https://www.gocardless.com/api/v2/accounts/{bankAccountId}/transactions";
            HttpClient client = new HttpClient();
            var body = new
            {
                secret_id = secret_id,
                secret_key = secret_key,
            };
            string json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            Console.WriteLine(JsonSerializer.Deserialize<AccessResponse>(response.Content.ReadAsStringAsync().Result).Access);
            return new CustomHTTPResponse((int)response.StatusCode,response.Content.ReadAsStringAsync().Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    
    private static string getSecretId()
    {
        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");
        var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

        Byte[]key = Convert.FromBase64String(config["gocardless:secret_id"]);
        return Encoding.UTF8.GetString(key);
        
    }

    
    public static async Task<CustomHTTPResponse> GetBanks(string country)
    {
        
        try
        {
            string url = $"https://bankaccountdata.gocardless.com/api/v2/institutions/?country={country}";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(response.StatusCode);
            return new CustomHTTPResponse((int)response.StatusCode,response.Content.ReadAsStringAsync().Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static async Task<CustomHTTPResponse> CreateAgreement(string bankAccountId)
    {
        
        try
        {
            string url = $"https://bankaccountdata.gocardless.com/api/v2/agreements/enduser/";
            HttpClient client = new HttpClient();
            var body = new { };
            string json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            Console.WriteLine(response.StatusCode);
            return new CustomHTTPResponse((int)response.StatusCode,response.Content.ReadAsStringAsync().Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static async Task<CustomHTTPResponse> CreateRequisition(string bankAccountId)
    {
        
        try
        {
            string url = $"https://bankaccountdata.gocardless.com/api/v2/requisitions/";
            HttpClient client = new HttpClient();
            var body = new { };
            string json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            Console.WriteLine(response.StatusCode);
            return new CustomHTTPResponse((int)response.StatusCode,response.Content.ReadAsStringAsync().Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static async Task<CustomHTTPResponse> GetBankDetailsRequisition(string idRequisition)
    {
        
        try
        {
            string url = $"https://bankaccountdata.gocardless.com/api/v2/requisitions/{idRequisition}/";
            HttpClient client = new HttpClient();
            var body = new { };
            string json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            Console.WriteLine(response.StatusCode);
            return new CustomHTTPResponse((int)response.StatusCode,response.Content.ReadAsStringAsync().Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}