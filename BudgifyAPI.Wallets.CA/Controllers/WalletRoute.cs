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

    
    public static WebApplication SetRoutes(WebApplication app, string baseRoute) {
    
        app.MapPost($"{baseRoute}/", async (HttpRequest req,[FromBody] WalletRequests body) => {
            try {
                
                var receivedUid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(receivedUid))
                {
                    return new CustomHttpResponse(){Status = 400,Message = "Missing token"};
                
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
               return await WalletInteractorEf.RegisterWallet(WalletPersistence.RegisterWallet, Guid.Parse(uid), body.WalletName);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        app.MapDelete($"{baseRoute}/{{walletId}}", async (HttpRequest req, Guid walletId) => {
            try {
                var receivedUid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(receivedUid))
                {
                    return new CustomHttpResponse(){Status = 400,Message = "Missing token"};
                
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
               return await WalletInteractorEf.DeleteWallet(WalletPersistence.DeleteWallet, Guid.Parse(uid), walletId);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        app.MapGet($"{baseRoute}/", async (HttpRequest req) => {
            try {
                var receivedUid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(receivedUid))
                {
                    return new CustomHttpResponse(){Status = 400,Message = "Missing token"};
                
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                Console.WriteLine(uid);
               return await WalletInteractorEf.GetWallet(WalletPersistence.GetWallet, Guid.Parse(uid));
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        app.MapPut($"{baseRoute}/", async (HttpRequest req, [FromBody] EditWalletRequest body) =>
        {
            try
            {
                var receivedUid = req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(receivedUid))
                {
                    return new CustomHttpResponse() { Status = 400, Message = "Missing token" };

                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                return await WalletInteractorEf.EditWallet(WalletPersistence.EditWallet, body,Guid.Parse(uid));
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
                var receivedUid  =req.Headers["X-User-Id"];
                if (string.IsNullOrEmpty(receivedUid))
                {
                    return new CustomHttpResponse() { Status = 400, Message = "Missing token" };
                }

                var uid = CustomEncryptor.DecryptString(
                    Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
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
        
        return app;
    }
}