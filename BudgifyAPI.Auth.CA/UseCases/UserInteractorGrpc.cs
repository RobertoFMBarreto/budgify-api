namespace BudgifyAPI.Auth.CA.UseCases;

public static class UserInteractorGrpc
{
    public static async Task<bool> LogoutEverything(Func<string,Task<bool>> userPersistence, string uid)
    {

        return await userPersistence(uid);
        
    }
}