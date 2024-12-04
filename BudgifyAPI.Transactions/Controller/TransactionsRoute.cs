using System.Text;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;
using BudgifyAPI.Transactions.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyAPI.Transactions.Controller
{
    public class TransactionsRoute
    {
        public static WebApplication SetRoutes(WebApplication application, string baseRoute)
        {
            application.MapPost($"{baseRoute}/", async ([FromBody] CreateTransaction transaction) =>
            {
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
            application.MapGet($"{baseRoute}/", async (HttpRequest req) =>
            {
                try
                {
                    var received_uid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {received_uid}");
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Bad Request",
                        };

                    }

                    Console.WriteLine($"Received uid: {Encoding.UTF8.GetString(Convert.FromBase64String(received_uid))}");
                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    Console.WriteLine($"uid: {uid}");
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactrions(TransactionsPersistence.GetTransactionsPersistence, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/transaction/{{limite}}/{{curindex}}", async (int limite, int cur_index, [FromBody] TransactionGroup transactionGroup) =>
            {
                //!TODO: Mudar para post para receber a referencia temporal
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactionSlidingWindow(TransactionsPersistence.GetTransactionSlidingWindowPersistence, transactionGroup);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/range", async ([FromBody] CreateTransaction transaction) =>
            {
                //!TODO: Mudar para post para receber o range
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactionNoSlidingWindow(TransactionsPersistence.GetTransactionNoSlidingWindowPersistence, transaction);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/transaction/{{transactionId}}/{{walletId}}", async (Guid transactionId, Guid walletId, [FromBody] CreateTransaction transaction) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.UpdateTrasnactions(TransactionsPersistence.UpdateTrasnactionsPersistence, transactionId, walletId, transaction);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/transaction/{{transactionId}}", async (Guid transactionId, Guid walletId) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.DeleteTrasnactions(TransactionsPersistence.DeleteTrasnactionsPersistence, transactionId, walletId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/categories", async () =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetCategories(TransactionsPersistence.GetCategoriesPersistence);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/categories", async ([FromBody] CreateCategories categories) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.AddCategories(TransactionsPersistence.AddCategoriesPersistence, categories);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/categories/{{categoryId}}", async (Guid categoryId, [FromBody] RequestName name) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.UpdateCategories(TransactionsPersistence.UpdateCategoriesPersistence, categoryId, name);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/categories/{{categoryId}}", async (Guid categoryId) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.DeleteCategory(TransactionsPersistence.DeleteCategoryPersistence, categoryId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/subcategories", async ([FromBody] CreateSubcategory subcategories) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.AddSubcategories(TransactionsPersistence.AddSubcategoriesPersistence, subcategories);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/subcategories/{{subcategoryId}}", async (Guid subcategoryId, [FromBody] RequestName name) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.UpdateSubcategories(TransactionsPersistence.UpdateSubcategoriesPersistence, subcategoryId, name);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/subcategories/{{subcategoryId}}", async (Guid subcategoryId) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.DeleteSubcategory(TransactionsPersistence.DeleteSubcategoryPersistence, subcategoryId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/reocurring", async ([FromBody] CreateReocurring reocurring) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.AddReocurring(TransactionsPersistence.AddReocurringPersistence, reocurring);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/reocurring/{{reocurringId}}", async (Guid reocurringId, [FromBody] CreateReocurring reocurring) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.UpdateReocurring(TransactionsPersistence.UpdateReocurringPersistence, reocurringId, reocurring);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/reocurring/{{reocurringId}}", async (Guid reocurringId) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.DeleteReocurring(TransactionsPersistence.DeleteReocurringPersistence, reocurringId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/transaciotnGroup", async ([FromBody] CreateTransactionGroup transactionGroup) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.AddTransactionGroup(TransactionsPersistence.AddTransactionGroupPersistence, transactionGroup);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/reocurring/{{transactionGroupId}}", async (Guid transactionGroupId, [FromBody] CreateTransactionGroup transactionGroup) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.UpdateTransactionGroup(TransactionsPersistence.UpdateTransactionGroupPersistence, transactionGroupId, transactionGroup);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/reocurring/{{transactionGroupId}}", async (Guid transactionGroupId) =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.DeleteTransactionGroup(TransactionsPersistence.DeleteTransactionGroupPersistence, transactionGroupId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/transactionGroup", async () =>
            {
                try
                {
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactionsGroup(TransactionsPersistence.GetTransactionsGroupPersistence);
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
