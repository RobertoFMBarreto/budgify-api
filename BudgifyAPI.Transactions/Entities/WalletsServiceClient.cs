using System.Text;
using System.Text.Json;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using Getwalletsgrpcservice;
using Grpc.Net.Client;

namespace BudgifyAPI.Transactions.UseCases;

public static class WalletsServiceClient
{
    public static async Task<IEnumerable<string>> GetUserWallets(Guid uid)
    {
       
        var channel = GrpcChannel.ForAddress(Encoding.UTF8.GetString(Convert.FromBase64String(Environment.GetEnvironmentVariable(
            "grpc__walletservice"))));
    
        GetWalletsGrpcService.GetWalletsGrpcServiceClient client =
            new GetWalletsGrpcService.GetWalletsGrpcServiceClient(channel);
        var request = new GetWalletsRequest() { Uid = uid.ToString() };
        var response = await client.GetWalletsAsync(request);
        return response.WalletId;
    }

    public static async Task<WalletRequest> GetUserWalletsHttp(Guid uid)
    {
        
        
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri($"http://budgify-wallets:65088"),
        };
        httpClient.DefaultRequestHeaders.Add("X-User-Id",Convert.ToBase64String(Encoding.UTF8.GetBytes(CustomEncryptor.EncryptString(uid.ToString()))));
        
        var resp = await httpClient.GetAsync("/api/v1/wallets");
        
        WalletRequest body = JsonSerializer.Deserialize<WalletRequest>(resp.Content.ReadAsStringAsync().Result);
        
        return body;

    }
    
    public static async Task<GetWalletByIdResponse> GetWalletById(Guid walletId, Guid uid)
    {
       
        var channel = GrpcChannel.ForAddress(Encoding.UTF8.GetString(Convert.FromBase64String(Environment.GetEnvironmentVariable(
            "grpc__walletservice"))));
    
        GetWalletsGrpcService.GetWalletsGrpcServiceClient client =
            new GetWalletsGrpcService.GetWalletsGrpcServiceClient(channel);
        var request = new GetWalletByIdRequest() { Uid = uid.ToString(),WalletId = walletId.ToString() };
        var response = await client.GetWalletByIDAsync(request);
        return response;
    }
}   