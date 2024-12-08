using System.Text;
using Authservice;
using BudgifyAPI.Auth.CA.UseCases;
using Grpc.Core;

namespace BudgifyAPI.Auth.CA.Entities;

public class AuthServiceHandler: Authservice.AuthService.AuthServiceBase
{
    public override async Task<LogoutUserResponse> LogoutUser(LogoutUserRequest request, ServerCallContext context)
    {
        var resp = await UserInteractorGrpc.LogoutEverything(UserPersistenceGrpc.LogoutEverythingPersistence, CustomEncryptor.DecryptString(Encoding.UTF8.GetString(Convert.FromBase64String(request.Uid))));
        return new LogoutUserResponse()
        {
            Done = resp
        };
    }

    public override async Task<ValidateTokenResponse> ValidateRefreshToken(ValidateTokenRequest request, ServerCallContext context)
    {
        var isValid = await ValidateToken(request);

        return new ValidateTokenResponse
        {
            IsValid = isValid,
        };
    }

    private async Task<bool> ValidateToken(ValidateTokenRequest request)
    {
        return await UserInteractorEf.ValidateSession(UserPersistence.ValidateSessionPersistence,request);
    }
}