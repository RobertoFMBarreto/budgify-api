using System.Text;
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
}   