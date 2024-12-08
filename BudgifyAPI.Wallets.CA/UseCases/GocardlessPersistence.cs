using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Entities.Requests;
using BudgifyAPI.Wallets.CA.Entities.Responses.GoCardless;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessPersistence
{
    public static async Task<Dictionary<string, object>> GetAccessTokenPersistence()
    {
        
        try
        {
            Byte[] secretKeyBytes =
                Convert.FromBase64String( Environment.GetEnvironmentVariable(
                    "gocardless__secret_key"));
            string secretKey= Encoding.UTF8.GetString(secretKeyBytes);
            
            Byte[] secretIdBytes =
                Convert.FromBase64String( Environment.GetEnvironmentVariable(
                    "gocardless__secret_id"));
            string secretId= Encoding.UTF8.GetString(secretIdBytes);
            
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
    
    public static async Task<CustomHttpResponse> GetBanksPersistence(string country)
    {
        
        try
        {
            Dictionary<string, object> accessResponse = await GetAccessTokenPersistence();
            
            string url = $"https://bankaccountdata.gocardless.com/api/v2/institutions/?country={country}";
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessResponse["access"].ToString()); 
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(requestMessage);
            List<Dictionary<string,object>> banks = JsonSerializer.Deserialize<List<Dictionary<string,object>>>(response.Content.ReadAsStringAsync().Result);
            
            return new CustomHttpResponse(){Status = (int)response.StatusCode, Data = banks};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static async Task<CustomHttpResponse> CreateAgreementPersistence(CreateAgreement createAgreement)
    {
        
        try
        {
            Dictionary<string, object> accessResponse = await GetAccessTokenPersistence();
            string url = $"https://bankaccountdata.gocardless.com/api/v2/agreements/enduser/";
            HttpClient client = new HttpClient();
            var body = new
            {
                institution_id = createAgreement.Intitution,
                max_historical_days = createAgreement.MaxHistoricalDays,
                access_valid_for_days = createAgreement.AccesValidForDays,
                access_scope = new List<string>{"balances", "details", "transactions"}
            };
            string json = JsonSerializer.Serialize(body);
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessResponse["access"].ToString());
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(requestMessage);
            
            Dictionary<string,object> agreement = JsonSerializer.Deserialize<Dictionary<string,object>>(response.Content.ReadAsStringAsync().Result);
            return new CustomHttpResponse(){Status =(int)response.StatusCode, Data = agreement };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static async Task<CustomHttpResponse> CreateRequisitionPersistence(CreateRequisitionRequest requisitionRequest,Guid userId)
    {
        
        try
        {
            Dictionary<string, object> accessResponse = await GetAccessTokenPersistence();
            string url = $"https://bankaccountdata.gocardless.com/api/v2/requisitions/";
            HttpClient client = new HttpClient();
            var body = new
            {
                redirect = requisitionRequest.Redirect,
                institution_id = requisitionRequest.InstitutionId,
                reference = userId.ToString(),
                agreement = requisitionRequest.Agreement,
                user_language = requisitionRequest.UserLanguage,
            };
            string json = JsonSerializer.Serialize(body);
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessResponse["access"].ToString());
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(requestMessage);
            Dictionary<string,object> requisition = JsonSerializer.Deserialize<Dictionary<string,object>>(response.Content.ReadAsStringAsync().Result);
            return new CustomHttpResponse(){Status =(int)response.StatusCode, Data =requisition };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static async Task<CustomHttpResponse> GetBankDetailsRequisitionPersistence(string idRequisition)
    {
        
        try
        {
            Dictionary<string, object> accessResponse = await GetAccessTokenPersistence();
            string url = $"https://bankaccountdata.gocardless.com/api/v2/requisitions/{idRequisition}/";
            HttpClient client = new HttpClient();
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessResponse["access"].ToString());
            var response = await client.SendAsync(requestMessage);
            Dictionary<string,object> bankDetails = JsonSerializer.Deserialize<Dictionary<string,object>>(response.Content.ReadAsStringAsync().Result);
            return new CustomHttpResponse(){Status =(int)response.StatusCode, Data =bankDetails };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
}