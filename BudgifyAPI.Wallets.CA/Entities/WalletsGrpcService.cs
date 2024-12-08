using BudgifyAPI.Wallets.CA.UseCases;
using Getwalletsgrpcservice;
using Grpc.Core;

namespace BudgifyAPI.Wallets.CA.Controllers;

public class WalletsGrpcService: Getwalletsgrpcservice.GetWalletsGrpcService.GetWalletsGrpcServiceBase
{
    public override async Task<GetWalletsResponse> GetWallets(GetWalletsRequest request, ServerCallContext context)
    {
       List<string> data = await GrpcInteractor.GetUserWalletsInteractor(GrpcPersistence.GetUserWalletsPersistence, Guid.Parse(request.Uid));
       var response = new GetWalletsResponse()
       {
           WalletId = { }
       };
       response.WalletId.AddRange(data);
       return response;
    }
}