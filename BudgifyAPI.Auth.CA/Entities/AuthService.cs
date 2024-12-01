using Authservice;
using BudgifyAPI.Auth.CA.UseCases;
using Grpc.Core;

namespace BudgifyAPI.Auth.CA.Entities;

public class AuthServiceHandler: Authservice.AuthService.AuthServiceBase
{
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
        return await UserInteractorEF.ValidateSession(UserPersistence.ValidateSessionPersistence,request);
    }
}