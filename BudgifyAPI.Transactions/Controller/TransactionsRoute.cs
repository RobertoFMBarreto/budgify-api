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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp = await TransactionsInteractorEf.AddTransaction(
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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.GetTransactions(
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
                    
                    try
                    {
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                        CustomHttpResponse resp = await TransactionsInteractorEf.GetTransactionSlidingWindow(
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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await TransactionsInteractorEf.GetTransactionNoSlidingWindow(
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                        CustomHttpResponse resp = await TransactionsInteractorEf.UpdateTransactions(
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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.DeleteTransactions(
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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.GetCategories(TransactionsPersistence.GetCategoriesPersistence,
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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.AddCategories(TransactionsPersistence.AddCategoriesPersistence,
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEf.UpdateCategories(
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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.DeleteCategory(TransactionsPersistence.DeleteCategoryPersistence,
                            categoryId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/subcategories", async (HttpRequest req) =>
            {
                try
                {
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.GetSubcategories(TransactionsPersistence.GetSubcategoriesPersistence,
                            Guid.Parse(uid));
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                        CustomHttpResponse resp =
                            await TransactionsInteractorEf.AddSubcategories(
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEf.UpdateSubcategories(
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEf.DeleteSubcategory(
                                TransactionsPersistence.DeleteSubcategoryPersistence, subcategoryId, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapGet($"{baseRoute}/reocurring", async (HttpRequest req) =>
            {
                try
                {
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.GetReocurring(TransactionsPersistence.GetReocurringPersistence,
                            Guid.Parse(uid));
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                        CustomHttpResponse resp =
                            await TransactionsInteractorEf.AddReocurring(
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                        CustomHttpResponse resp = await TransactionsInteractorEf.UpdateReocurring(
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                        CustomHttpResponse resp =
                            await TransactionsInteractorEf.DeleteReocurring(
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
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                        CustomHttpResponse resp = await TransactionsInteractorEf.AddTransactionGroup(
                            TransactionsPersistence.AddTransactionGroupPersistence, transactionGroup, Guid.Parse(uid));
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            application.MapPut($"{baseRoute}/transaction-group/{{transactionGroupId}}",
                async (HttpRequest req, Guid transactionGroupId, [FromBody] CreateTransactionGroup transactionGroup) =>
                {
                    try
                    {
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                        CustomHttpResponse resp = await TransactionsInteractorEf.UpdateTransactionGroup(
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
            application.MapDelete($"{baseRoute}/transaction-group/{{transactionGroupId}}",
                async (HttpRequest req, Guid transactionGroupId) =>
                {
                    try
                    {
                        var receivedUid = req.Headers["X-User-Id"];
                        Console.WriteLine($"Received uid: {receivedUid}");
                        if (string.IsNullOrEmpty(receivedUid))
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Bad Request",
                            };
                        }

                        var uid = CustomEncryptor.DecryptString(
                            Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                        CustomHttpResponse resp =
                            await TransactionsInteractorEf.DeleteTransactionGroup(
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
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp =
                        await TransactionsInteractorEf.GetTransactionsGroup(TransactionsPersistence
                            .GetTransactionsGroupPersistence, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            
            application.MapGet($"{baseRoute}/gocardless/{{idWallet}}/transactions", async (HttpRequest req, Guid idWallet) => {
                try {
                    var receivedUid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {receivedUid}");
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    return await GocardlessInteractor.GetTransactionsInteractor(GocardlessPersistence.GetTransactionsPersistence, Guid.Parse(uid),idWallet);
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
            
            return application;
        }
    }
}