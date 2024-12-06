using System.Text;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.UseCases;
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
                    return new CustomHttpResponse(){status = 400,message = "Missing token"};
                
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
                    return new CustomHttpResponse(){status = 400,message = "Missing token"};
                
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
                    return new CustomHttpResponse(){status = 400,message = "Missing token"};
                
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                await GocardlessInteractor.GetBanksInteractor(GocardlessPersistence.GetBanksPersistence,"pt");
               return await WalletInteractorEF.GetWallet(WalletPersistence.GetWallet, Guid.Parse(uid));
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapGet($"{baseRoute}/gocardless/banks/{{country}}", async (string country) => {
            try {
                return await GocardlessInteractor.GetBanksInteractor(GocardlessPersistence.GetBanksPersistence,country);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapPost($"{baseRoute}/gocardless/agreement", async ([FromBody] CreateAgreement createAgreement) => {
            try {
                return await GocardlessInteractor.CreateAgreementInteractor(GocardlessPersistence.CreateAgreementPersistence,createAgreement);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapPost($"{baseRoute}/gocardless/requisition", async (HttpRequest req, [FromBody] CreateRequisitionRequest requisitionRequest) => {
            try {
                var received_uid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(received_uid))
                {
                    return new CustomHttpResponse() { status = 400, message = "Missing token" };
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                return await GocardlessInteractor.CreateRequisitionnteractor(GocardlessPersistence.CreateRequisitionPersistence, requisitionRequest,Guid.Parse(uid));
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapGet($"{baseRoute}/gocardless/bank/accounts/{{idRequisition}}", async (HttpRequest req, string idRequisition) => {
            try {
                return await GocardlessInteractor.GetBankDetailsRequisitionInteractor(GocardlessPersistence.GetBankDetailsRequisitionPersistence, idRequisition);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        app.MapGet($"{baseRoute}/gocardless/bank/accounts/{{idAccount}}/transactions/", async (HttpRequest req, string idAccount) => {
            try {
                return await GocardlessInteractor.GetTransactionsInteractor(GocardlessPersistence.GetTransactionsPersistence, idAccount);
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