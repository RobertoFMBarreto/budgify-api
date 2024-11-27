using Microsoft.AspNetCore.Mvc;
using BudgifyAPI.Wallets.CA.Entities.Requests;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.UseCases;


namespace BudgifyAPI.Wallets.CA.Controllers;

public static class WalletRoute {

    public static WebApplication setRoutes(WebApplication app, string baseRoute) {
        app.MapPost($"{baseRoute}/RegisterWallet", async ([FromBody] RegisterWalletRequest body) => {
            try {
               // CustomHTTPResponse resp = await WalletInteractorEF.
                
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