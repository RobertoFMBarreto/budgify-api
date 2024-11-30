using Microsoft.AspNetCore.Mvc;
using BudgifyAPI.Wallets.CA.Entities.Requests;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.UseCases;


namespace BudgifyAPI.Wallets.CA.Controllers;

public static class WalletRoute {

    
    public static WebApplication setRoutes(WebApplication app, string baseRoute) {
    
        Guid tempUserID = Guid.NewGuid();
    
    
        app.MapPost($"{baseRoute}/RegisterWallet", async ([FromBody] RegisterWalletRequest body) => {
            try {
               return await WalletInteractorEF.RegisterWallet(WalletPersistence.RegisterWallet, tempUserID, body.wallet_name);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        app.MapDelete($"{baseRoute}/DeleteWallet", async ([FromBody] DeleteWalletRequest body) => {
            try {
               return await WalletInteractorEF.DeleteWallet(WalletPersistence.DeleteWallet, tempUserID, body.wallet_id);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        app.MapGet($"{baseRoute}/GetWallet", async ([FromBody] GetWalletRequest body) => {
            try {
               return await WalletInteractorEF.GetWallet(WalletPersistence.GetWallet, tempUserID, body.wallet_id);
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