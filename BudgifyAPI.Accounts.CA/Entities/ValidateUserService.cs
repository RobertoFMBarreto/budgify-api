using System.Text;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using BudgifyAPI.Accounts.CA.UsesCases;
using Grpc.Core;
using Validateuserservice;

namespace BudgifyAPI.Accounts.CA.Entities;

public class ValidateUserServiceHandler : Validateuserservice.ValidateUserService.ValidateUserServiceBase
{
    public override async Task<ValidateUserResponse> ValidateUserService(ValidateUserRequest request,
        ServerCallContext context)
    {
        
        Console.WriteLine($"Validating user: {request.Uid}");
        User? user = null;
        if (!string.IsNullOrEmpty(request.Uid))
        {
            user = await AccountsInteractorGrpc.GetUserByIdGrpc(AccountsPersistenceGrpc.GetUserByIdPersistenceGrpc,
                Guid.Parse(
                    CustomEncryptor.DecryptString(Encoding.UTF8.GetString(Convert.FromBase64String(request.Uid)))));
        }
        else if (request.Password != null && request.Email != null)
        {
            user = await AccountsInteractorGrpc.ValidadeUserInteractor(AccountsPersistenceGrpc.ValidateUserPersistence,
                CustomEncryptor.DecryptString(Encoding.UTF8.GetString(Convert.FromBase64String(request.Email))),
                CustomEncryptor.DecryptString(Encoding.UTF8.GetString(Convert.FromBase64String(request.Password))));
            if (user == null)
                return new ValidateUserResponse()
                {
                    IsValid = false,
                };
        }
        return user == null
            ? new ValidateUserResponse()
            {
                Uid = request.Uid,
                IsValid = false,
            }
            : new ValidateUserResponse()
            {
                Uid = user.IdUser.ToString(),
                IsAdmin = user.IsAdmin,
                IsManager = user.IsManager,
                IsSuperadmin = user.IsSuperAdmin,
                IsValid = true,
            };
    }
}