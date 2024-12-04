using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgifyAPI.Transactions.UseCases
{
    public class TransactionsPersistence
    {
        public static async Task<CustomHttpResponse> AddTransactionPersistence(CreateTransaction transaction)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                
                //string query = "SELECT tg.@id_transaction_ " +
                //        "FROM public.transactions as t inner join public.transaction_group as tg " +
                //        "on t.id_transaction_group = tg.id_transaction_group " +
                //        "WHERE t.date BETWEEN @tg.start_date AND @tg.end_date ";

                //var verify = await transactionsContext.TransactionGroups.FromSqlRaw(query,
                //        new NpgsqlParameter("@start_date", transactionGroup.StartDate),
                //        new NpgsqlParameter("@end_date", transactionGroup.EndDate), 
                //        new NpgsqlParameter("@id_transaction", transactionId)).ToListAsync();

                //if (string.IsNullOrEmpty(query))
                //{
                //    return new CustomHttpResponse()
                //    {
                //        message = "Transaction Group does not exist",
                //        status = 400
                //    };
                //}
                //else
                //{
                //    await transactionsContext.AddAsync(new Transaction
                //    {
                //        IdWallet = transaction.IdWallet,
                //        IdTransactionGroup = transactionGroupId,
                //        IdCategory = transaction.IdCategory,
                //        IdSubcategory = transaction.IdSubcategory,
                //        IdReocurring = transaction.IdReocurring,
                //        Date = transaction.Date,
                //        Description = transaction.Description,
                //        Amount = transaction.Amount,
                //        IsPlanned = transaction.IsPlanned,
                //        Latitude = transaction.Latitude,
                //        Longitue = transaction.Longitue,
                //    });
                //}
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
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> GetTrasnactionsIntervalPersistence()
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.transactions " +
                    "where date = CURRENT_TIMESTAMP - INTERVAL '7' " +
                    "ORDER BY date DESC";
                List<Transaction> listTransaction = await transactionsContext.Transactions.FromSqlRaw(query).ToListAsync();
                return new CustomHttpResponse()
                {
                    Data = listTransaction,
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
        public static async Task<CustomHttpResponse> GetTransactionSlidingWindowPersistence(TransactionGroup transactionGroup, int limite, int cur_index)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            //ver
            {
                try
                {
                    string query = "SELECT tg.*, t.amount " +
                        "FROM public.transactions as t inner join public.transaction_group as tg " +
                        "on t.id_transaction_group = tg.id_transaction_group " +
                        "WHERE t.date BETWEEN @tg.start_date AND @tg.end_date " +
                        "ORDER BY @t.date ASC " +
                        "LIMIT @limite OFFSET @cur_index";

                    List<Transaction> listTransactions = await transactionsContext.Transactions.FromSqlRaw(query,
                        new NpgsqlParameter("@start_date", transactionGroup.StartDate),
                        new NpgsqlParameter("@end_date", transactionGroup.EndDate),
                        new NpgsqlParameter("@limite", limite),
                        new NpgsqlParameter("@cur_index", cur_index)).ToListAsync();
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
        public static async Task<CustomHttpResponse> GetTransactionNoSlidingWindowPersistence(CreateTransaction transaction)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                //TODO
                return new CustomHttpResponse()
                {

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
                    status= 200
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
                        message = "transaction does not exist",
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

        public static async Task<CustomHttpResponse> GetGroupTransactionStats(TransactionGroup transactionGroup, Guid transactionId)
        {
            return new CustomHttpResponse()
            {

            };
            //TransactionsContext transactionsContext = new TransactionsContext();
            //try
            //{
            //    var existTransaction = await transactionsContext.TransactionGroups.FirstOrDefaultAsync(x => x.IdTransactionGroup == transactionId);
            //    string query = "";
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);  
            //    throw;
            //}
        }


    }
}
