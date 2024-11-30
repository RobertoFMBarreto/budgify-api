using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;

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
    
    public static async Task<CustomHttpResponse> register(Func<UserEntity,Task<CustomHttpResponse>> userPersistence, UserEntity user)
    {
        return await userPersistence(user);
        
    }

    public static async Task<CustomHttpResponse> newPassword(
        Func<NewPasswordRequest, Task<CustomHttpResponse>> newPasswordPersistence,
        NewPasswordRequest newPasswordRequest)
    {
        return await newPasswordPersistence(newPasswordRequest);
    }
    
    public static async Task<CustomHttpResponse> newSessionToken(
        Func<NewSessionTokenRequest, Task<CustomHttpResponse>> newSessionTokenPersistence,
        NewSessionTokenRequest
            newSessionTokenRequest)
    {
        return await newSessionTokenPersistence(newSessionTokenRequest);
    }
}