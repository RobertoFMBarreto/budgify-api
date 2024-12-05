using Getwalletsgrpcservice;
using Grpc.Net.Client;

namespace BudgifyAPI.Transactions.UseCases;

public static class WalletsServiceClient
{
    public static async Task<IEnumerable<string>> GetUserWallets(Guid uid)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7132");
    
        GetWalletsGrpcService.GetWalletsGrpcServiceClient _client =
            new GetWalletsGrpcService.GetWalletsGrpcServiceClient(channel);
        var request = new GetWalletsRequest() { Uid = uid.ToString() };
        var response = await _client.GetWalletsAsync(request);
        return response.WalletId;
    }
}   