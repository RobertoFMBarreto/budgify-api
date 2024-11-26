using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;
using BudgifyAPI.Auth.CA.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyAPI.Auth.CA.controllers;

public static class UserRoute
{
    public static WebApplication setRoutes(WebApplication app, string baseRoute)
    {
        app.MapPost($"{baseRoute}/login", async ([FromBody]LoginRequest body)=>
        {
            try
            {
                CustomHttpResponse resp = await UserInteractorEF.login(UserPersistence.UserLoginPersistence,body.Email,body.Password );
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