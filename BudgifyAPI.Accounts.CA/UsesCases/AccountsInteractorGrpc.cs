using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;

namespace BudgifyAPI.Accounts.CA.UsesCases;

public static class AccountsInteractorGrpc
{
    public static async Task<User?> GetUserByIdGrpc(Func<Guid, Task<User?>> getUserByIdPersistenceGrpc, Guid userId)
    {
        return await getUserByIdPersistenceGrpc(userId);
    }
    
    public static async Task<User?> ValidadeUserInteractor(Func<string,string, Task<User?>> validateUserPersistence, string email, string password)
    {
        return await validateUserPersistence(email, password);
    }
}