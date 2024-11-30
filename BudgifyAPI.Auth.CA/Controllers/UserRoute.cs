using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;
using BudgifyAPI.Auth.CA.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyAPI.Auth.CA.controllers;

public static class UserRoute
{
    public static WebApplication SetRoutes(WebApplication app, string baseRoute)
    {
        app.MapPost($"{baseRoute}/login", [AllowAnonymous] async (HttpRequest req,[FromBody] LoginRequest body) =>
        {
            var uid  =req.Headers["X-User-Id"];

            if (!string.IsNullOrEmpty(uid))
            {
                Console.WriteLine($"Received; {System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(uid))}");
                Console.WriteLine(
                    $"Received; {CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(uid)))}");
            }

            try
            {
                CustomHttpResponse resp =
                    await UserInteractorEF.login(UserPersistence.UserLoginPersistence, body.Email, body.Password);
                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapPost($"{baseRoute}/register",[AllowAnonymous] async ([FromBody]UserEntity body)=>
        {
            try
            {
                CustomHttpResponse resp = await UserInteractorEF.register(UserPersistence.UserRegisterPersistence,body );
                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapPost($"{baseRoute}/new-password",[AllowAnonymous] async ([FromBody]NewPasswordRequest body)=>
        {
            try
            {
                CustomHttpResponse resp = await UserInteractorEF.newPassword(UserPersistence.NewPasswordPersistence,body );
                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        
        app.MapPost($"{baseRoute}/new-session-token",[AllowAnonymous] async ([FromBody]NewSessionTokenRequest body)=>
        {
            try
            {
                CustomHttpResponse resp = await UserInteractorEF.newSessionToken(UserPersistence.NewSessionTokenPersistence,body );
                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        return app;
    }
}