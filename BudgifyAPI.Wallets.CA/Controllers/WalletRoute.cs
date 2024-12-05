using System.Text;
using BudgifyAPI.Auth.CA.Entities;
using Microsoft.AspNetCore.Mvc;
using BudgifyAPI.Wallets.CA.Entities.Requests;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.UseCases;


namespace BudgifyAPI.Wallets.CA.Controllers;

public static class WalletRoute {

    
    public static WebApplication setRoutes(WebApplication app, string baseRoute) {
    
        Guid tempUserID = Guid.NewGuid();
    
    
        app.MapPost($"{baseRoute}/", async (HttpRequest req,[FromBody] RegisterWalletRequest body) => {
            try {
                
                var received_uid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(received_uid))
                {
                    return new CustomHTTPResponse(400,"Missing token");
                
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
               return await WalletInteractorEF.RegisterWallet(WalletPersistence.RegisterWallet, Guid.Parse(uid), body.wallet_name);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        app.MapDelete($"{baseRoute}/", async (HttpRequest req,[FromBody] DeleteWalletRequest body) => {
            try {
                var received_uid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(received_uid))
                {
                    return new CustomHTTPResponse(400,"Missing token");
                
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
               return await WalletInteractorEF.DeleteWallet(WalletPersistence.DeleteWallet, Guid.Parse(uid), body.wallet_id);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        app.MapGet($"{baseRoute}/", async (HttpRequest req) => {
            try {
                var received_uid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(received_uid))
                {
                    return new CustomHTTPResponse(400,"Missing token");
                
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
               return await WalletInteractorEF.GetWallet(WalletPersistence.GetWallet, Guid.Parse(uid));
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