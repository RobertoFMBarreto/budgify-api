using System.Data;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;
using Getwalletsgrpcservice;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;
using System.Security.Cryptography.X509Certificates;
using BudgifyAPI.Transactions.Entities.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection.Metadata.Ecma335;

namespace BudgifyAPI.Transactions.UseCases
{
    public class TransactionsPersistence
    {
        public static async Task<CustomHttpResponse> AddTransactionPersistence(CreateTransaction transaction,
            Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                if (transaction.IdTransactionGroup != null)
                {
                    var validateGroup = await transactionsContext.TransactionGroups.Where(tg =>
                            tg.IdTransactionGroup == transaction.IdTransactionGroup && tg.IdUser == userId)
                        .FirstOrDefaultAsync();
                    if (validateGroup == null)
                    {
                        return new CustomHttpResponse()
                        {
                            Message = "Transaction Group does not exist",
                            Status = 400
                        };
                    }
                    
                    if (transaction.IdCategory != null)
                    {
                        var validateCat = await transactionsContext.Categories.FirstOrDefaultAsync(x =>
                            x.IdCategory == transaction.IdCategory && x.IdUser == userId);
                        if (validateCat == null)
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Category does not exist"
                            };
                        }
                    }

                    if (transaction.IdCategory != null && transaction.IdSubcategory != null)
                    {
                        var validateSubCat = await transactionsContext.Subcategories.FirstOrDefaultAsync(x =>
                            x.IdSubcategory == transaction.IdSubcategory && x.IdCategory == transaction.IdCategory &&
                            x.IdUser == userId);
                        if (validateSubCat == null)
                        {
                            return new CustomHttpResponse()
                            {
                                Status = 400,
                                Message = "Sub category does not exist"
                            };
                        }
                    }

                    await transactionsContext.AddAsync(new Transaction
                    {
                        IdWallet = transaction.IdWallet,
                        IdTransactionGroup = transaction.IdTransactionGroup,
                        IdCategory = transaction.IdCategory,
                        IdSubcategory = transaction.IdSubcategory,
                        IdReocurring = transaction.IdReocurring,
                        Date = transaction.Date,
                        Description = transaction.Description,
                        Amount = transaction.Amount,
                        IsPlanned = transaction.IsPlanned,
                        Latitude = transaction.Latitude,
                        Longitude = transaction.Longitue,
                    });
                    await transactionsContext.SaveChangesAsync();
                    return new CustomHttpResponse()
                    {
                        Message = "Transaction added success",
                        Status = 200,
                    };
                }
                else
                {
                    string query2 = " SELECT * " +
                                    "FROM public.transaction_group tg " +
                                    "WHERE @date BETWEEN tg.start_date AND tg.end_date";

                    var isOccurringGroup = await transactionsContext.TransactionGroups.FromSqlRaw(query2,
                        new NpgsqlParameter("@date", transaction.Date)).FirstOrDefaultAsync();

                    if (isOccurringGroup == null)
                    {
                        await transactionsContext.Transactions.AddAsync(new Transaction
                        {
                            IdWallet = transaction.IdWallet,
                            IdCategory = transaction.IdCategory,
                            IdSubcategory = transaction.IdSubcategory,
                            IdReocurring = transaction.IdReocurring,
                            Date = transaction.Date,
                            Description = transaction.Description,
                            Amount = transaction.Amount,
                            IsPlanned = transaction.IsPlanned,
                            Latitude = transaction.Latitude,
                            Longitude = transaction.Longitue,
                        });
                        await transactionsContext.SaveChangesAsync();

                        return new CustomHttpResponse()
                        {
                            Message = "Transaction added success",
                            Status = 200
                        };
                    }

                    await transactionsContext.Transactions.AddAsync(new Transaction
                    {
                        IdWallet = transaction.IdWallet,
                        IdCategory = transaction.IdCategory,
                        IdSubcategory = transaction.IdSubcategory,
                        IdReocurring = transaction.IdReocurring,
                        IdTransactionGroup = isOccurringGroup.IdTransactionGroup,
                        Date = transaction.Date,
                        Description = transaction.Description,
                        Amount = transaction.Amount,
                        IsPlanned = transaction.IsPlanned,
                        Latitude = transaction.Latitude,
                        Longitude = transaction.Longitue,
                    });
                    await transactionsContext.SaveChangesAsync();
                    return new CustomHttpResponse()
                    {
                        Message = "Transaction added success",
                        Status = 200
                    };
                }


                // tem tranction group? (idTransactionGroup = null?)
                // sim
                //  Verificar se o transaction group existe e é do user
                //  Inserir a transação
                // não
                //  Verificar se existe algum transaction group do user que tenha a data da transação entre start data e end date do grupo
                //  Sim
                //      Inserir transação com o id do transaction group indicado (Se houver mais do que um adicionamos ao primeiro)
                //  Não
                //      Inserir a transação como uma transação normal sem grupo
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> GetTransactionsIntervalPersistence(Guid uid)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(uid);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                var parameters = guids
                    .Select((uuid, index) => new NpgsqlParameter($"@uuid{index}", uuid))
                    .ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));

                string query = "select * from public.transactions " +
                               "where date >= (CURRENT_TIMESTAMP - INTERVAL '7 DAY')::DATE::TIMESTAMP " +
                               "AND date <= (CURRENT_TIMESTAMP + INTERVAL '7 DAY')::DATE::TIMESTAMP " +
                               $"AND id_wallet IN ({parameterNames}) " +
                               "ORDER BY date DESC";


                List<Transaction> listTransaction = await transactionsContext.Transactions.FromSqlRaw(query,
                    parameters.ToArray()
                ).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listTransaction,
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> GetTransactionSlidingWindowPersistence(int limite, int curIndex,
            Guid userId, DateTime? firstItemDate = null)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            //ver
            {
                try
                {
                    IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                    string[] walletsArray = wallets.ToArray();
                    Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                    var parameters = guids
                        .Select((uuid, index) => new NpgsqlParameter($"@uuid{index}", uuid))
                        .ToList();
                    var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                    parameters.AddRange([
                        new NpgsqlParameter("@Limite", limite), new NpgsqlParameter("@CurIndex", curIndex)
                    ]);

                    if (curIndex == 0)
                    {
                        string queryAllTransactions = "SELECT * " +
                                                      "FROM public.transactions as t " +
                                                      $"WHERE id_wallet IN ({parameterNames}) " +
                                                      "ORDER BY t.date DESC " +
                                                      "LIMIT @Limite OFFSET @CurIndex";


                        List<Transaction> listAllTransactions = await transactionsContext.Transactions.FromSqlRaw(
                            queryAllTransactions,
                            parameters.ToArray()).ToListAsync();


                        string queryTransactions = "SELECT * " +
                                                   "FROM public.transactions as t " +
                                                   "WHERE t.id_transaction_group IS NULL " +
                                                   $"AND id_wallet IN ({parameterNames}) " +
                                                   "ORDER BY t.date DESC " +
                                                   "LIMIT @Limite OFFSET @CurIndex";


                        List<Transaction> listTransactions = await transactionsContext.Transactions.FromSqlRaw(
                            queryTransactions,
                            parameters.ToArray()).ToListAsync();
                        List<TransactionGroup> transactionGroups=new List<TransactionGroup>();
                        if (listAllTransactions.Count != 0)
                        {
                            var firsttransDate = listAllTransactions.First().Date;
                            var lastTransDate = listAllTransactions.Last().Date;

                            parameters.AddRange([
                                new NpgsqlParameter("@IdUser", userId),
                                new NpgsqlParameter("@StartDate", lastTransDate),
                                new NpgsqlParameter("@EndDate", firsttransDate)
                            ]);
                            string queryTransactionGroup = "SELECT tg.* " +
                                                           "FROM public.transaction_group as tg " +
                                                           "INNER JOIN public.transactions as t  ON t.id_transaction_group = tg.id_transaction_group " +
                                                           "WHERE tg.id_user = @IdUser " +
                                                           $"AND t.date BETWEEN @StartDate AND @EndDate " +
                                                           "Group BY tg.id_transaction_group " +
                                                           "LIMIT @Limite OFFSET @CurIndex";


                            transactionGroups.AddRange(await transactionsContext.TransactionGroups
                                .FromSqlRaw(queryTransactionGroup,
                                    parameters.ToArray()).ToListAsync());
                        }

                        return new CustomHttpResponse()
                        {
                            Data = new TransactionsResponse()
                            {
                                Transactions = listTransactions,
                                TransactionGroups = transactionGroups,
                            },
                            Status = 200
                        };
                    }
                    else
                    {
                        parameters.AddRange([
                            new NpgsqlParameter("@FirstItemDate", firstItemDate)
                        ]);
                        string queryAllTransactions = "SELECT * " +
                                                      "FROM public.transactions as t " +
                                                      $"WHERE id_wallet IN ({parameterNames}) AND t.date <= @FirstItemDate" +
                                                      "ORDER BY t.date DESC " +
                                                      "LIMIT @Limite OFFSET @CurIndex";


                        List<Transaction> listAllTransactions = await transactionsContext.Transactions.FromSqlRaw(
                            queryAllTransactions,
                            parameters.ToArray()).ToListAsync();
                        
                        if (firstItemDate == null)
                        {
                            return new CustomHttpResponse()
                            {
                                Message = "Date of the first item is required for subsequent requests.",
                                Status = 400
                            };
                        }

                        string queryTransactions = "SELECT * " +
                                                   "FROM public.transactions as t " +
                                                   "WHERE t.id_transaction_group IS NULL AND t.date <= @FirstItemDate " +
                                                   "AND t.id_wallet IN ({parameterNames}) " +
                                                   "ORDER BY t.date DESC " +
                                                   "LIMIT @Limite OFFSET @CurIndex";


                        List<Transaction> listTransactions = await transactionsContext.Transactions.FromSqlRaw(
                            queryTransactions,
                            parameters.ToArray()).ToListAsync();
                        List<TransactionGroup> transactionGroups=new List<TransactionGroup>();
                        if (listAllTransactions.Count != 0)
                        {
                            var firsttransDate = listAllTransactions.First().Date;
                            var lastTransDate = listAllTransactions.Last().Date;

                            parameters.AddRange([
                                new NpgsqlParameter("@IdUser", userId),
                                new NpgsqlParameter("@StartDate", lastTransDate),
                                new NpgsqlParameter("@EndDate", firsttransDate)
                            ]);
                            string queryTransactionGroup = "SELECT tg.* " +
                                                           "FROM public.transaction_group as tg " +
                                                           "INNER JOIN public.transactions as t  ON t.id_transaction_group = tg.id_transaction_group " +
                                                           "WHERE t.date <= @FirstItemDate AND tg.id_user = @IdUser " +
                                                           $"AND t.date BETWEEN @StartDate AND @EndDate " +
                                                           "Group BY tg.id_transaction_group " +
                                                           "LIMIT @Limite OFFSET @CurIndex";
                            transactionGroups.AddRange(await transactionsContext.TransactionGroups
                                .FromSqlRaw(queryTransactionGroup,
                                    parameters.ToArray()).ToListAsync());
                        }

                        return new CustomHttpResponse()
                        {
                            Data = new TransactionsResponse()
                            {
                                Transactions = listTransactions,
                                TransactionGroups = transactionGroups,
                            },
                            Status = 200
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return new CustomHttpResponse()
                    {
                        Message = ex.Message,
                        Status = 500
                    };
                }
            }
        }

        public static async Task<CustomHttpResponse> GetTransactionNoSlidingWindowPersistence(DateTime startDate,
            DateTime endDate, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                //WalletRequest wallets = await WalletsServiceClient.GetUserWalletsHttp(userId);
                //string[] walletsArray = wallets.data.Select(x=> x.idWallet).ToArray();
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                var parameters = guids
                    .Select((uuid, index) => new NpgsqlParameter($"@uuid{index}", uuid))
                    .ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));

                parameters.AddRange([new NpgsqlParameter("@IdUser", userId), new NpgsqlParameter("@StartDate", startDate),new NpgsqlParameter("@EndDate", endDate),]);
                string queryAllTransactions = "SELECT * " +
                                              "FROM public.transactions as t " +
                                              $"WHERE id_wallet IN ({parameterNames}) AND t.date BETWEEN @StartDAte AND @EndDate " +
                                              "ORDER BY t.date DESC ";


                List<Transaction> listAllTransactions = await transactionsContext.Transactions.FromSqlRaw(
                    queryAllTransactions,
                    parameters.ToArray()).ToListAsync();

                
                string queryTransactions = "SELECT * " +
                                           "FROM public.transactions as t " +
                                           "WHERE t.id_transaction_group IS NULL AND t.date BETWEEN @StartDAte AND @EndDate " +
                                           $"AND id_wallet IN ({parameterNames}) " +
                                           "ORDER BY t.date DESC ";


                List<Transaction> listTransactions = await transactionsContext.Transactions.FromSqlRaw(
                    queryTransactions,
                    parameters.ToArray()).ToListAsync();
                List<TransactionGroup> transactionGroups=new List<TransactionGroup>();
                if (listAllTransactions.Count != 0)
                {
                    var firsttransDate = listAllTransactions.First().Date;
                    var lastTransDate = listAllTransactions.Last().Date;

                    parameters.AddRange([
                        new NpgsqlParameter("@IdUser", userId), new NpgsqlParameter("@StartDate", lastTransDate),
                        new NpgsqlParameter("@EndDate", firsttransDate)
                    ]);
                    string queryTransactionGroup = "SELECT tg.* " +
                                                   "FROM public.transaction_group as tg " +
                                                   "INNER JOIN public.transactions as t  ON t.id_transaction_group = tg.id_transaction_group " +
                                                   "WHERE tg.id_user = @IdUser " +
                                                   $"AND t.date BETWEEN @StartDate AND @EndDate " +
                                                   "Group BY tg.id_transaction_group";


                    transactionGroups.AddRange(await transactionsContext.TransactionGroups.FromSqlRaw(
                        queryTransactionGroup,
                        parameters.ToArray()).ToListAsync());
                }

                return new CustomHttpResponse()
                {
                    Data = new TransactionsResponse()
                    {
                        Transactions = listTransactions,
                        TransactionGroups = transactionGroups,
                    },
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> UpdateTransactionsPersistence(Guid transactionId,Guid userId,
            CreateTransaction transaction)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                var validation = await transactionsContext.Transactions.FirstOrDefaultAsync(x =>
                    x.IdTransaction == transactionId && guids.Contains(x.IdWallet));
                if (validation == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Transaction does not exist",
                        Status = 400
                    };
                }

                if (transaction.IdCategory != null)
                {
                    var validateCat = await transactionsContext.Categories.FirstOrDefaultAsync(x =>
                        x.IdCategory == transaction.IdCategory && x.IdUser == userId);
                    if (validateCat == null)
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Category does not exist"
                        };
                    }
                }

                if (transaction.IdCategory != null && transaction.IdSubcategory != null)
                {
                    var validateSubCat = await transactionsContext.Subcategories.FirstOrDefaultAsync(x =>
                        x.IdSubcategory == transaction.IdSubcategory && x.IdCategory == transaction.IdCategory &&
                        x.IdUser == userId);
                    if (validateSubCat == null)
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Sub category does not exist"
                        };
                    }
                }

                validation.IdWallet = transaction.IdWallet;
                validation.IdReocurring = transaction.IdReocurring;
                validation.IdCategory = transaction.IdCategory;
                validation.IdSubcategory = transaction.IdSubcategory;
                validation.Date = transaction.Date;
                validation.Description = transaction.Description;
                validation.Amount = transaction.Amount;
                validation.IsPlanned = transaction.IsPlanned;
                validation.Latitude = transaction.Latitude;
                validation.Longitude = transaction.Longitue;

                transactionsContext.Update(validation);
                await transactionsContext.SaveChangesAsync();

                return new CustomHttpResponse()
                {
                    Message = "Transaction updated successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> DeleteTransactionsPersistence(Guid transactionId, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                
                var validation = await transactionsContext.Transactions.FirstOrDefaultAsync(x =>
                    x.IdTransaction == transactionId && guids.Contains(x.IdWallet));
                if (validation == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Transaction does not exist",
                        Status = 400
                    };
                }

                transactionsContext.Transactions.Remove(validation);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Transaction removed successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> GetCategoriesPersistence(Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.category WHERE id_user = @IdUser";
                List<Category> listCategory = await transactionsContext.Categories.FromSqlRaw(query, new NpgsqlParameter("@IdUser", userId)).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listCategory,
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> AddCategoriesPersistence(RequestName category, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                await transactionsContext.AddAsync(new Category
                {
                    Name = category.Name,
                    IdUser = userId,
                });
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Category added successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> UpdateCategoriesPersistence(Guid categoryId, RequestName name, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var categoryExist =
                    await transactionsContext.Categories.FirstOrDefaultAsync(x => x.IdCategory == categoryId && x.IdUser == userId);
                if (categoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Category does not exist",
                        Status = 400
                    };
                }

                categoryExist.Name = name.Name;
                transactionsContext.Categories.Update(categoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Categor updated successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> DeleteCategoryPersistence(Guid categoryId, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var categoryExist =
                    await transactionsContext.Categories.FirstOrDefaultAsync(x => x.IdCategory == categoryId && x.IdUser == userId);
                if (categoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Category does not exist",
                        Status = 400
                    };
                }

                transactionsContext.Categories.Remove(categoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Category removed successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> GetSubcategoriesPersistence(Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.subcategory WHERE id_user = @IdUser";
                List<Subcategory> listSubcategory = await transactionsContext.Subcategories.FromSqlRaw(query, new NpgsqlParameter("@IdUser", userId)).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listSubcategory,
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddSubcategoriesPersistence(CreateSubcategory subcategory, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                await transactionsContext.AddAsync(new Subcategory
                {
                    IdCategory = subcategory.IdCategory,
                    Name = subcategory.Name,
                    IdUser = userId,
                });
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Subcategory added successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> UpdateSubcategoriesPersistence(Guid subcategoryId,
            RequestName name, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var subcategoryExist =
                    await transactionsContext.Subcategories.FirstOrDefaultAsync(x => x.IdSubcategory == subcategoryId && x.IdUser == userId);
                if (subcategoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Subcategory does not exist",
                        Status = 400
                    };
                }

                subcategoryExist.Name = name.Name;
                transactionsContext.Subcategories.Update(subcategoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Subategory updated successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> DeleteSubcategoryPersistence(Guid subcategoryId, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var subcategoryExist =
                    await transactionsContext.Subcategories.FirstOrDefaultAsync(x => x.IdSubcategory == subcategoryId && x.IdUser == userId);
                if (subcategoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "A subcategory does not exist",
                        Status = 400
                    };
                }

                transactionsContext.Subcategories.Remove(subcategoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Subcategory removed successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> GetReocurringPersistence(Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                
                
                var parameters = guids
                    .Select((uuid, index) => new NpgsqlParameter($"@uuid{index}", uuid))
                    .ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                

                string query = $"select * from public.reocurring WHERE id_wallet IN ({parameterNames})";
                List<Reocurring> listReocurrings = await transactionsContext.Reocurrings.FromSqlRaw(query, parameters.ToArray()).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listReocurrings,
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddReocurringPersistence(CreateReocurring reocurring, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();

                if (!guids.Contains(reocurring.IdWallet))
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Wallet does not exist"
                    };
                }
                
                if (reocurring.IdCategory != null)
                {
                    var validateCat = await transactionsContext.Categories.FirstOrDefaultAsync(x =>
                        x.IdCategory == reocurring.IdCategory && x.IdUser == userId);
                    if (validateCat == null)
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Category does not exist"
                        };
                    }
                }

                if (reocurring.IdCategory != null && reocurring.IdSubcategory != null)
                {
                    var validateSubCat = await transactionsContext.Subcategories.FirstOrDefaultAsync(x =>
                        x.IdSubcategory == reocurring.IdSubcategory && x.IdCategory == reocurring.IdCategory &&
                        x.IdUser == userId);
                    if (validateSubCat == null)
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Sub category does not exist"
                        };
                    }
                }
                
                await transactionsContext.AddAsync(new Reocurring
                {
                    IdWallet = reocurring.IdWallet,
                    IdCategory = reocurring.IdCategory,
                    IdSubcategory = reocurring.IdSubcategory,
                    Description = reocurring.Description,
                    Amount = reocurring.Amount,
                    DayOfWeek = reocurring.DayOfWeek,
                    StartDate = reocurring.StartDate,
                    IsYearly = reocurring.IsYearly,
                    IsMonthly = reocurring.IsMonthly,
                    IsWeekly = reocurring.IsWeekly,
                    IsActive = reocurring.IsActive
                });
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Reocurring added successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> UpdateReocurringPersistence(Guid reocurringId,
            CreateReocurring reocurring, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                
                var reocurringExist =
                    await transactionsContext.Reocurrings.FirstOrDefaultAsync(x => x.IdReocurring == reocurringId && guids.Contains(x.IdWallet));
                if (reocurringExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Reocurring does not exist",
                        Status = 400
                    };
                }
                
                

                if (!guids.Contains(reocurring.IdWallet))
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Wallet does not exist"
                    };
                }
                
                if (reocurring.IdCategory != null)
                {
                    var validateCat = await transactionsContext.Categories.FirstOrDefaultAsync(x =>
                        x.IdCategory == reocurring.IdCategory && x.IdUser == userId);
                    if (validateCat == null)
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Category does not exist"
                        };
                    }
                }

                if (reocurring.IdCategory != null && reocurring.IdSubcategory != null)
                {
                    var validateSubCat = await transactionsContext.Subcategories.FirstOrDefaultAsync(x =>
                        x.IdSubcategory == reocurring.IdSubcategory && x.IdCategory == reocurring.IdCategory &&
                        x.IdUser == userId);
                    if (validateSubCat == null)
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Sub category does not exist"
                        };
                    }
                }

                reocurringExist.IdWallet = reocurring.IdWallet;
                reocurringExist.IdCategory = reocurring.IdCategory;
                reocurringExist.IdSubcategory = reocurring.IdSubcategory;
                reocurringExist.Description = reocurring.Description;
                reocurringExist.Amount = reocurring.Amount;
                reocurringExist.DayOfWeek = reocurring.DayOfWeek;
                reocurringExist.StartDate = reocurring.StartDate;
                reocurringExist.IsYearly = reocurring.IsYearly;
                reocurringExist.IsMonthly = reocurring.IsMonthly;
                reocurringExist.IsWeekly = reocurring.IsWeekly;
                reocurringExist.IsActive = reocurring.IsActive;

                transactionsContext.Reocurrings.Update(reocurringExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Reocurring updated successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> DeleteReocurringPersistence(Guid reocurringId, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                IEnumerable<string> wallets = await WalletsServiceClient.GetUserWallets(userId);
                string[] walletsArray = wallets.ToArray();
                Guid[] guids = walletsArray.Select(x => Guid.Parse(x)).ToArray();
                var reocurringExist =
                    await transactionsContext.Reocurrings.FirstOrDefaultAsync(x => x.IdReocurring == reocurringId && guids.Contains(x.IdWallet));
                if (reocurringExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Reocurring does not exist",
                        Status = 400
                    };
                }

                transactionsContext.Reocurrings.Remove(reocurringExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Reocurring removed successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> GetTransactionGroupPersistence(Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.transaction_group where user_id =@IdUser";
                List<TransactionGroup> listTransaGroup = await transactionsContext.TransactionGroups.FromSqlRaw(query, new NpgsqlParameter("@IdUser", userId)).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listTransaGroup,
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };

            }
        }
        public static async Task<CustomHttpResponse> AddTransactionGroupPersistence(
            CreateTransactionGroup transactionGroup, Guid uid)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                await transactionsContext.AddAsync(new TransactionGroup
                {
                    Description = transactionGroup.Description,
                    StartDate = transactionGroup.StartDate,
                    EndDate = transactionGroup.EndDate,
                    PlannedAmount = transactionGroup.PlannedAmount,
                    IdUser = uid
                });
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Transaction Group added successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> UpdateTransactionGroupPersistence(Guid transacationGroupId,
            CreateTransactionGroup transactionGroup, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var transactionGroupExist =
                    await transactionsContext.TransactionGroups.FirstOrDefaultAsync(x =>
                        x.IdTransactionGroup == transacationGroupId && x.IdUser == userId);
                if (transactionGroupExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Transaction Group does not exist",
                        Status = 400
                    };
                }

                transactionGroupExist.Description = transactionGroup.Description;
                transactionGroupExist.StartDate = transactionGroup.StartDate;
                transactionGroupExist.EndDate = transactionGroup.EndDate;
                transactionGroupExist.PlannedAmount = transactionGroup.PlannedAmount;
                 transactionsContext.TransactionGroups.Update(transactionGroupExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Transaction group updated successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> DeleteTransactionGroupPersistence(Guid transacationGroupId, Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var transactionGroupExist =
                    await transactionsContext.TransactionGroups.FirstOrDefaultAsync(x =>
                        x.IdTransactionGroup == transacationGroupId && x.IdUser == userId);
                if (transactionGroupExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Transaction Group does not exist",
                        Status = 400
                    };
                }

                transactionsContext.TransactionGroups.Remove(transactionGroupExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Transaction Group removed successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> GetTransactionsGroupPersistence(Guid userId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.transaction_group WHERE id_user = @IdUser";
                List<TransactionGroup> listTransacationgroup =
                    await transactionsContext.TransactionGroups.FromSqlRaw(query, new NpgsqlParameter("@IdUser", userId)).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listTransacationgroup,
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }

        public static async Task<CustomHttpResponse> ExportTransactions(Guid userId, Guid walletId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();

            try
            {
                string query = "select * from public.transactions t "
                    + "left join category c On c.id_category = t.id_category "+
                    "left join subcategory sc On sc.id_subcategory = t.id_subcategory " + 
                    "where t.id_wallet = @IdWallet";
                List<TransactionGroup> listTransacationgroup =
                    await transactionsContext.TransactionGroups.FromSqlRaw(query, 
                        new NpgsqlParameter("@IdWallet", walletId)).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listTransacationgroup,
                    Status = 200
                };
            }catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }
    }
}