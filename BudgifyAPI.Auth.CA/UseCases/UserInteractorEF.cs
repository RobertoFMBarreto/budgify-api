using Authservice;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;

namespace BudgifyAPI.Auth.CA.UseCases;

public static class UserInteractorEF
{
    public static async Task<CustomHttpResponse> login(Func<UserEntity,string,Task<CustomHttpResponse>> userPersistence, string email,
        string password, string userAgent)
    {
        UserEntity user = new UserEntity()
        {
            Email = email,
            Password = password
        };
        return await userPersistence(user,userAgent);
        
    }
    public static async Task<CustomHttpResponse> logout(Func<string,string,Task<CustomHttpResponse>> userPersistence, string uid,
        string userAgent)
    {

        return await userPersistence(uid,userAgent);
        
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

    public static async Task<bool> ValidateSession(Func<ValidateTokenRequest, Task<bool>> validateTokenPersistence,
        ValidateTokenRequest validateTokenRequest)
    {
        return await validateTokenPersistence(validateTokenRequest);
    }
}