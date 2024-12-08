using BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;

namespace BudgifyAPI.Auth.CA.UseCases;

public static class UserPersistenceGrpc
{
    public static async Task<bool> LogoutEverythingPersistence(string uid)
    { 
        AuthenticationContext context = new AuthenticationContext();
        try
        {
            
            context.UserRefreshTokens.RemoveRange(context.UserRefreshTokens.Where(ur=>ur.IdUser==Guid.Parse(uid)));
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
    }
}