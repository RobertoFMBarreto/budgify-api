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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BudgifyAPI.Transactions.UseCases
{
    public class TransactionsPersistence
    {
        public static async Task<CustomHttpResponse> AddTransactionPersistence(CreateTransaction transaction, TransactionGroup transactionGroup, DateTime date, Guid transactionGropupId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "SELECT @id_transaction_group " +
                        "FROM public.transaction_group " +
                        "WHERE @id_transaction_group = null ";

                var verify = await transactionsContext.TransactionGroups.FromSqlRaw(query,
                        new NpgsqlParameter("@id_transaction_group", transactionGropupId)).ToListAsync();

                if (string.IsNullOrEmpty(query))
                {
                    return new CustomHttpResponse()
                    {
                        message = "Transaction Group does not exist",
                        status = 400
                    };
                }
                else
                {
                    await transactionsContext.AddAsync(new Transaction
                    {
                        IdWallet = transaction.IdWallet,
                        IdTransactionGroup = transactionGropupId,
                        IdCategory = transaction.IdCategory,
                        IdSubcategory = transaction.IdSubcategory,
                        IdReocurring = transaction.IdReocurring,
                        Date = transaction.Date,
                        Description = transaction.Description,
                        Amount = transaction.Amount,
                        IsPlanned = transaction.IsPlanned,
                        Latitude = transaction.Latitude,
                        Longitue = transaction.Longitue,
                    });
                    await transactionsContext.SaveChangesAsync();
                  
                }
                string query2 = " SELECT @id_transaction_group " +
                    "FROM public.transaction_group " +
                    "WHERE @date BETWEEN @start_date AND @end_date";
                    
                var verify2 = await transactionsContext.TransactionGroups.FromSqlRaw(query,
                    new NpgsqlParameter("@start_date", transactionGroup.StartDate),
                    new NpgsqlParameter("@end_date", transactionGroup.EndDate),
                    new NpgsqlParameter("@id_transaction_group", transactionGropupId),
                    new NpgsqlParameter("@date", date)).FirstOrDefaultAsync();

                if(string.IsNullOrEmpty(query2))
                {
                    return new CustomHttpResponse()
                    {
                        message = "Transaction Group does not exist",
                        status = 400
                    };
                }
                else
                {
                    await transactionsContext.AddAsync(new Transaction
                    {
                        IdWallet = transaction.IdWallet,
                        IdTransactionGroup = transactionGropupId,
                        IdCategory = transaction.IdCategory,
                        IdSubcategory = transaction.IdSubcategory,
                        IdReocurring = transaction.IdReocurring,
                        Date = transaction.Date,
                        Description = transaction.Description,
                        Amount = transaction.Amount,
                        IsPlanned = transaction.IsPlanned,
                        Latitude = transaction.Latitude,
                        Longitue = transaction.Longitue,

                    });
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

                await transactionsContext.Transactions.AddAsync(new Transaction
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
                    Longitue = transaction.Longitue,
                });
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Transaction added success",
                    status = 200,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
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
                Guid[] guids = walletsArray.Select(x=>Guid.Parse(x)).ToArray();
                var parameters = guids
                    .Select((uuid, index) => new NpgsqlParameter($"@uuid{index}", uuid))
                    .ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                
                string query = "select * from public.transactions " +
                               "where date >= (CURRENT_TIMESTAMP - INTERVAL '7 DAY')::DATE::TIMESTAMP " +
                               $"AND id_wallet IN ({parameterNames}) " +
                               "ORDER BY date DESC";
     
                
                List<Transaction> listTransaction = await transactionsContext.Transactions.FromSqlRaw(query,
                        parameters.ToArray()
                    ).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listTransaction,
                    status = 200
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> GetTransactionSlidingWindowPersistence(TransactionGroup transactionGroup, int limite, int cur_index, DateTime? firstItemDate = null)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            //ver
            {
                try
                {
                    // Se for o primeiro pedido (cur_index == 0?)
                    // SIM
                    //  query normal sliding window
                    // Não
                    //  Receber data do primeiro item
                    //  query onde date <= data do primeiro item
                    
                    /* LIMIT 5 | cur_index 0    | -> 21:38
                     * 1 -> 21:37               | 1 -> 21:37
                     * 3 -> 21:35               | 2 -> 21:36
                     * 4 -> 21:34
                     * 5 -> 21:33
                     *
                     * LIMIT 5 | cur_index 6
                     * 6 -> 21:32
                     * 7 -> 21:31
                     * 8 -> 21:30
                     * 9 -> 21:29
                     * 10-> 21:28
                     * 
                     */
                    
                   if(cur_index == 0)
                   {
                       string query2 = "SELECT tg.*, t.amount " +
                           "FROM public.transactions as t inner join public.transaction_group as tg " +
                           "on t.id_transaction_group = tg.id_transaction_group " +
                           "WHERE t.date BETWEEN @tg.start_date AND @tg.end_date " +
                           "ORDER BY @t.date ASC " +
                           "LIMIT @limite OFFSET @cur_index";

                       List<Transaction> listTransactions2 = await transactionsContext.Transactions.FromSqlRaw(query2,
                           new NpgsqlParameter("@start_date", transactionGroup.StartDate),
                           new NpgsqlParameter("@end_date", transactionGroup.EndDate),
                           new NpgsqlParameter("@limite", limite),
                           new NpgsqlParameter("@cur_index", cur_index)).ToListAsync();

                       return new CustomHttpResponse()
                       {
                            Data = listTransactions2,
                            status = 200
                       };
                   }
                    else if (firstItemDate == null)
                    {
                        return new CustomHttpResponse()
                        {
                            message = "Date of the first item is required for subsequent requests.",
                            status = 400
                        };
                    }

                    string query = "SELECT tg.*, t.amount " +
                            "FROM public.transactions as t " +
                            "INNER JOIN public.transaction_group as tg " +
                            "ON t.id_transaction_group = tg.id_transaction_group " +
                            "WHERE t.date BETWEEN @tg.start_date AND @tg.end_date " +
                            "ORDER BY @t.date ASC " +
                            "LIMIT @limite OFFSET @cur_index";

                    List<Transaction> listTransactions = await transactionsContext.Transactions.FromSqlRaw(query,
                        new NpgsqlParameter("start_date", transactionGroup.StartDate),
                        new NpgsqlParameter("end_date", transactionGroup.EndDate),
                        new NpgsqlParameter("limite", limite),
                        new NpgsqlParameter("cur_index", cur_index),
                        new NpgsqlParameter("last_item_date", firstItemDate)).ToListAsync();

                    return new CustomHttpResponse()
                    {
                        Data = listTransactions,
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    return new CustomHttpResponse()
                    {
                        message = ex.Message,
                        status = 500
                    };
                }
            }
        }
        public static async Task<CustomHttpResponse> GetTransactionNoSlidingWindowPersistence(CreateTransaction transaction, DateTime date, Guid walletId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.transaction_group as tg " +
                    "inner join public.transactions as t on t.id_transaction_group = tg.id_transaction_group" +
                    "where t.id_wallet = @walletId AND @date BETWEEN start_date AND end_date";

                var listTransactions = await transactionsContext.TransactionGroups.FromSqlRaw(query, 
                    new NpgsqlParameter("@date", date),
                    new NpgsqlParameter("@walletId",walletId)).ToListAsync();

                return new CustomHttpResponse()
                {
                    Data = listTransactions,
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> UpdateTrasnactionsPersistence(Guid transactionId, Guid walletId, CreateTransaction transaction)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var validation = await transactionsContext.Transactions.FirstOrDefaultAsync(x => x.IdTransaction == transactionId && x.IdWallet == walletId);
                if (validation == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Transaction does not exist",
                        status = 400
                    };

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
                validation.Longitue = transaction.Longitue;

                transactionsContext.Update(validation);
                await transactionsContext.SaveChangesAsync();

                return new CustomHttpResponse()
                {
                    message = "Transaction updated successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteTrasnactionsPersistence(Guid transactionId, Guid walletId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                //var existWallet = await transactionsContext.Transactions.FirstOrDefaultAsync(x => x.IdWallet == walletId);
                //if (existWallet == null)
                //{
                //    return new CustomHttpResponse()
                //    {
                //        message = "Não existe carteira",
                //        status = 400
                //    };

                //}
                var validation = await transactionsContext.Transactions.FirstOrDefaultAsync(x => x.IdTransaction == transactionId && x.IdWallet == walletId);
                if (validation == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Transaction does not exist",
                        status = 400
                    };

                }
                transactionsContext.Transactions.Remove(validation);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Transaction removed successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }

        }
        public static async Task<CustomHttpResponse> GetCategoriesPersistence()
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.categories";
                List<Category> listCategory = await transactionsContext.Categories.FromSqlRaw(query).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listCategory,
                    status = 200
                };

            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddCategoriesPersistence(CreateCategories category)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                await transactionsContext.AddAsync(new Category
                {
                    Name = category.Name,
                    IdUser = category.IdUser,
                });
                return new CustomHttpResponse()
                {
                    message = "Category added successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> UpdateCategoriesPersistence(Guid categoryId, RequestName name)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var categoryExist = await transactionsContext.Categories.FirstOrDefaultAsync(x => x.IdCategory == categoryId);
                if (categoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Category does not exist",
                        status = 400
                    };
                }
                categoryExist.Name = name.Name;
                transactionsContext.Categories.Update(categoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Categor updated successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteCategoryPersistence(Guid categoryId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var categoryExist = await transactionsContext.Categories.FirstOrDefaultAsync(x => x.IdCategory == categoryId);
                if (categoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Category does not exist",
                        status = 400
                    };
                }
                transactionsContext.Categories.Remove(categoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Category removed successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddSubcategoriesPersistence(CreateSubcategory subcategory)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                await transactionsContext.AddAsync(new Subcategory
                {
                    IdCategory = subcategory.IdCategory,
                    Name = subcategory.Name,
                    IdUser = subcategory.IdUser,
                });
                return new CustomHttpResponse()
                {
                    message = "Subcategory added successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> UpdateSubcategoriesPersistence(Guid subcategoryId, RequestName name)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var subcategoryExist = await transactionsContext.Subcategories.FirstOrDefaultAsync(x => x.IdSubcategory == subcategoryId);
                if (subcategoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Subcategory does not exist",
                        status = 400
                    };
                }
                subcategoryExist.Name = name.Name;
                transactionsContext.Subcategories.Update(subcategoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Subategory updated successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteSubcategoryPersistence(Guid subcategoryId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var subcategoryExist = await transactionsContext.Subcategories.FirstOrDefaultAsync(x => x.IdSubcategory == subcategoryId);
                if (subcategoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "A subcategory does not exist",
                        status = 400
                    };
                }
                transactionsContext.Subcategories.Remove(subcategoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Subcategory removed successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddReocurringPersistence(CreateReocurring reocurring)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
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
                return new CustomHttpResponse()
                {
                    message = "Reocurring added successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> UpdateReocurringPersistence(Guid reocurringId, CreateReocurring reocurring)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var reocurringExist = await transactionsContext.Reocurrings.FirstOrDefaultAsync(x => x.IdReocurring == reocurringId);
                if (reocurringExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Reocurring does not exist",
                        status = 400
                    };
                }

                reocurring.IdWallet = reocurring.IdWallet;
                reocurring.IdCategory = reocurring.IdCategory;
                reocurring.IdSubcategory = reocurring.IdSubcategory;
                reocurring.Description = reocurring.Description;
                reocurring.Amount = reocurring.Amount;
                reocurring.DayOfWeek = reocurring.DayOfWeek;
                reocurring.StartDate = reocurring.StartDate;
                reocurring.IsYearly = reocurring.IsYearly;
                reocurring.IsMonthly = reocurring.IsMonthly;
                reocurring.IsWeekly = reocurring.IsWeekly;
                reocurring.IsActive = reocurring.IsActive;

                transactionsContext.Reocurrings.Update(reocurringExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Reocurring updated successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteReocurringPersistence(Guid reocurringId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var reocurringExist = await transactionsContext.Reocurrings.FirstOrDefaultAsync(x => x.IdReocurring == reocurringId);
                if (reocurringExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Reocurring does not exist",
                        status = 400
                    };
                }
                transactionsContext.Reocurrings.Remove(reocurringExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Reocurring removed successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddTransactionGroupPersistence(CreateTransactionGroup transactionGroup)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                await transactionsContext.AddAsync(new TransactionGroup
                {
                    Description = transactionGroup.Description,
                    StartDate = transactionGroup.StartDate,
                    EndDate = transactionGroup.EndDate,
                    PlannedAmount = transactionGroup.PlannedAmount
                });
                return new CustomHttpResponse()
                {
                    message = "Transaction Group added successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> UpdateTransactionGroupPersistence(Guid transacationGroupId, CreateTransactionGroup transactionGroup)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var transacationGroupExist = await transactionsContext.TransactionGroups.FirstOrDefaultAsync(x => x.IdTransactionGroup == transacationGroupId);
                if (transacationGroupExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Transaction Group does not exist",
                        status = 400
                    };
                }
                transacationGroupExist.Description = transactionGroup.Description;
                transacationGroupExist.StartDate = transactionGroup.StartDate;
                transacationGroupExist.EndDate = transactionGroup.EndDate;
                transacationGroupExist.PlannedAmount = transactionGroup.PlannedAmount;

                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Transaction group updated successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteTransactionGroupPersistence(Guid transacationGroupId)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                var transactionGroupExist = await transactionsContext.TransactionGroups.FirstOrDefaultAsync(x => x.IdTransactionGroup == transacationGroupId);
                if (transactionGroupExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Transaction Group does not exist",
                        status = 400
                    };
                }
                transactionsContext.TransactionGroups.Remove(transactionGroupExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Transaction Group removed successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> GetTransactionsGroupPersistence()
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.transaction_group";
                List<TransactionGroup> listTransacationgroup = await transactionsContext.TransactionGroups.FromSqlRaw(query).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listTransacationgroup,
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> GetGroupTransactionStatsPersistence(Guid transactionGroupId)
        {
            TransactionsContext transactionContext = new TransactionsContext(); 
            try
            {
                string query = "select " +
               "COUNT(tg.id_transaction_group) as TOTAL_COUNT, " +
               "MAX(t.amount) as MAX, MIN(t.amount) as " +
               "MIN, SUM(t.amount) as TOTAL_SUM, " +
               "AVG(t.amount) AS MEDIA " +
               "from public.transaction_group as tg " +
               "inner join public.transactions as t on tg.id_transaction_group = t.id_transaction_group " +
               "where tg.id_transaction_group = @transactionGroupId";

                var result = transactionContext.TransactionGroups.FromSqlRaw(query, new NpgsqlParameter("@transactionGroupId", transactionGroupId));

                return new CustomHttpResponse()
                {
                    Data = result,
                    status = 200
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
                throw;
            }
        }
        public static async Task<CustomHttpResponse> GetGroupTransactionStatsRange(Guid transactionGroupId)
        {
            TransactionsContext transactionContext = new TransactionsContext();
            try
            {
                string query = "select " +
               "COUNT(tg.id_transaction_group) as TOTAL_COUNT, " +
               "MAX(t.amount) as MAX, MIN(t.amount) as " +
               "MIN, SUM(t.amount) as TOTAL_SUM, " +
               "AVG(t.amount) AS MEDIA " +
               "from public.transaction_group as tg " +
               "inner join public.transactions as t on tg.id_transaction_group = t.id_transaction_group " +
               "where tg.id_transaction_group = @transactionGroupId tg.start_date <= tg.end_date";

                var result = transactionContext.TransactionGroups.FromSqlRaw(query, 
                    new NpgsqlParameter("@transactionGroupId", transactionGroupId));

                return new CustomHttpResponse()
                {
                    Data = result,
                    status = 200
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
                throw;
            }
        }

    }
}
