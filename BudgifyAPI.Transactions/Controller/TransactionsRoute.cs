using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyAPI.Transactions.Controller
{
    public class TransactionsRoute
    {
        public static WebApplication SetRoutes(WebApplication application, string baseRoute)
        {
            application.MapPost("{baseRoute}/transaction", async ([FromBody] CreateTransaction transaction) => {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.AddTransaction(TransactionsPersistence.AddTransactionPersistence, transaction);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/transaction", async () =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactrions(TransactionsPersistence.GetTransactrionsPersistence);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/transaction", async () =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactionSlidingWindow(TransactionsPersistence.GetTransactionSlidingWindowPersistence);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/transaction", async ([FromBody] CreateTransaction transaction) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTrasnactionsInterval(TransactionsPersistence.GetTrasnactionsIntervalPersistence, transaction);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/transaction", async ([FromBody] CreateTransaction transaction) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.UpdateTrasnactions(TransactionsPersistence.UpdateTrasnactionsPersistence, transaction);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            return application;
        }
    }
}
