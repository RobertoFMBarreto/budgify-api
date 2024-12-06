using System.Text;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;
using BudgifyAPI.Transactions.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Transactions.Controller
{
    public class TransactionsRoute
    {
        public static WebApplication SetRoutes(WebApplication application, string baseRoute)
        {
            application.MapPost($"{baseRoute}/", async (HttpRequest req, [FromBody] CreateTransaction transaction) =>
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp = await TransactionsInteractorEF.AddTransaction(
                        TransactionsPersistence.AddTransactionPersistence, transaction, Guid.Parse(uid));
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp =
                        await TransactionsInteractorEF.GetTransactions(
                            TransactionsPersistence.GetTransactionsIntervalPersistence, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/{{limite}}/{{curindex}}",
                async (HttpRequest req, int limite, int curindex, [FromBody] DateRequest date) =>
                {
                    //!TODO: Mudar para post para receber a referencia temporal
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                        CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactionSlidingWindow(
                            TransactionsPersistence.GetTransactionSlidingWindowPersistence, limite, curindex, date.Date,
                            Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapPost($"{baseRoute}/range", async (HttpRequest req, [FromBody] DateIntervalRequest dates) =>
            {
                //!TODO: Mudar para post para receber o range
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await TransactionsInteractorEF.GetTransactionNoSlidingWindow(
                        TransactionsPersistence.GetTransactionNoSlidingWindowPersistence, dates, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/{{transactionId}}",
                async (HttpRequest req, Guid transactionId, [FromBody] CreateTransaction transaction) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                        CustomHttpResponse resp = await TransactionsInteractorEF.UpdateTransactions(
                            TransactionsPersistence.UpdateTransactionsPersistence, transactionId, transaction,
                            Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapDelete($"{baseRoute}/{{transactionId}}", async (HttpRequest req, Guid transactionId) =>
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEF.DeleteTransactions(
                            TransactionsPersistence.DeleteTransactionsPersistence, transactionId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/categories", async (HttpRequest req) =>
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEF.GetCategories(TransactionsPersistence.GetCategoriesPersistence,
                            Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/categories", async (HttpRequest req, [FromBody] RequestName categories) =>
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp =
                        await TransactionsInteractorEF.AddCategories(TransactionsPersistence.AddCategoriesPersistence,
                            categories, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/categories/{{categoryId}}",
                async (HttpRequest req, Guid categoryId, [FromBody] RequestName name) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEF.UpdateCategories(
                                TransactionsPersistence.UpdateCategoriesPersistence, categoryId, name, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapDelete($"{baseRoute}/categories/{{categoryId}}", async (HttpRequest req, Guid categoryId) =>
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEF.DeleteCategory(TransactionsPersistence.DeleteCategoryPersistence,
                            categoryId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/subcategories",
                async (HttpRequest req, [FromBody] CreateSubcategory subcategories) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                        CustomHttpResponse resp =
                            await TransactionsInteractorEF.AddSubcategories(
                                TransactionsPersistence.AddSubcategoriesPersistence, subcategories, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapPut($"{baseRoute}/subcategories/{{subcategoryId}}",
                async (HttpRequest req, Guid subcategoryId, [FromBody] RequestName name) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEF.UpdateSubcategories(
                                TransactionsPersistence.UpdateSubcategoriesPersistence, subcategoryId, name,
                                Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapDelete($"{baseRoute}/subcategories/{{subcategoryId}}",
                async (HttpRequest req, Guid subcategoryId) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEF.DeleteSubcategory(
                                TransactionsPersistence.DeleteSubcategoryPersistence, subcategoryId, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapPost($"{baseRoute}/reocurring",
                async (HttpRequest req, [FromBody] CreateReocurring reocurring) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                        CustomHttpResponse resp =
                            await TransactionsInteractorEF.AddReocurring(
                                TransactionsPersistence.AddReocurringPersistence,
                                reocurring, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapPut($"{baseRoute}/reocurring/{{reocurringId}}",
                async (HttpRequest req, Guid reocurringId, [FromBody] CreateReocurring reocurring) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                        CustomHttpResponse resp = await TransactionsInteractorEF.UpdateReocurring(
                            TransactionsPersistence.UpdateReocurringPersistence, reocurringId, reocurring,
                            Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapDelete($"{baseRoute}/reocurring/{{reocurringId}}",
                async (HttpRequest req, Guid reocurringId) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEF.DeleteReocurring(
                                TransactionsPersistence.DeleteReocurringPersistence, reocurringId, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapPost($"{baseRoute}/transaction-group",
                async (HttpRequest req, [FromBody] CreateTransactionGroup transactionGroup) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                        CustomHttpResponse resp = await TransactionsInteractorEF.AddTransactionGroup(
                            TransactionsPersistence.AddTransactionGroupPersistence, transactionGroup, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapPut($"{baseRoute}/reocurring/{{transactionGroupId}}",
                async (HttpRequest req, Guid transactionGroupId, [FromBody] CreateTransactionGroup transactionGroup) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                        CustomHttpResponse resp = await TransactionsInteractorEF.UpdateTransactionGroup(
                            TransactionsPersistence.UpdateTransactionGroupPersistence, transactionGroupId,
                            transactionGroup, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapDelete($"{baseRoute}/reocurring/{{transactionGroupId}}",
                async (HttpRequest req, Guid transactionGroupId) =>
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

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                        CustomHttpResponse resp =
                            await TransactionsInteractorEF.DeleteTransactionGroup(
                                TransactionsPersistence.DeleteTransactionGroupPersistence, transactionGroupId,
                                Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapGet($"{baseRoute}/transaction-group", async (HttpRequest req) =>
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

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp =
                        await TransactionsInteractorEF.GetTransactionsGroup(TransactionsPersistence
                            .GetTransactionsGroupPersistence, Guid.Parse(uid));
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