using BudgifyAPI.Auth.CA.Entities;

namespace BudgifyAPI.Auth.CA.UseCases;

public static class UserInteractorEF
{
    public static async Task<CustomHttpResponse> login(Func<UserEntity,Task<CustomHttpResponse>> userPersistence, string email,
        string password)
    {
        UserEntity user = new UserEntity()
        {
            Email = email,
            Password = password
        };
        return await userPersistence(user);
        
    }
}