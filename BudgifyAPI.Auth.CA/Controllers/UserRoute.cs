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
        app.MapPost($"{baseRoute}/login", [AllowAnonymous] async (HttpContext context,HttpRequest req,[FromBody] LoginRequest body) =>
        {
            var userAgent = req.Headers["User-Agent"];
            var host = req.Headers["X-User-Ip"];
            if (host.Contains(":"))
            {
                host = "127.0.0.1";
            }
            try
            {
                CustomHttpResponse resp =
                    await UserInteractorEf.Login(UserPersistence.UserLoginPersistence, body.Email, body.Password,$"{userAgent} / {host}");
                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapGet($"{baseRoute}/logout", [AllowAnonymous] async (HttpContext context,HttpRequest req) =>
        {
            var userAgent = req.Headers["User-Agent"];
            var host = req.Headers["X-User-Ip"];
            var receivedUid  =req.Headers["X-User-Id"];
            if (host.Contains(":"))
            {
                host = "127.0.0.1";
            }
            if (string.IsNullOrEmpty(receivedUid))
            {
                return new CustomHttpResponse()
                {
                    Status = 400,
                    Message = "Missing token",
                };
                
                
            }
            var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
            try
            {
                CustomHttpResponse resp =
                    await UserInteractorEf.Logout(UserPersistence.UserLogoutPersistence, uid, $"{userAgent} / {host}");
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
                CustomHttpResponse resp = await UserInteractorEf.NewSessionToken(UserPersistence.NewSessionTokenPersistence,body );
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