using BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;
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

    public override async Task<GetWalletByIdResponse> GetWalletByID(GetWalletByIdRequest request, ServerCallContext context)
    {
        Wallet? wallet = await GrpcInteractor.GetWalletsById(GrpcPersistence.GetWalletByIdPersistence, Guid.Parse(request.WalletId),Guid.Parse(request.Uid));
        return wallet==null? new GetWalletByIdResponse()
        {
            AccountId = null,
            StoreInCloud = false
        }: new GetWalletByIdResponse()
        {
            AccountId = wallet.IdAccount,
            StoreInCloud = wallet.StoreInCloud
        };
    }
}