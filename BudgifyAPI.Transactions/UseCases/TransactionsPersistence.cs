﻿using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace BudgifyAPI.Transactions.UseCases
{
    public class TransactionsPersistence
    {
        public static async Task<CustomHttpResponse> AddTransactionPersistence(CreateTransaction transaction)
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                await transactionsContext.AddAsync(new Transaction
                {
                    IdWallet = transaction.IdWallet,
                    IdCategory = transaction.IdCategory,
                    IdSubcategory = transaction.IdSubcategory,
                    IdTransactionGroup = transaction.IdTransactionGroup,
                    IdReocurring = transaction.IdReocurring,
                    Date = transaction.Date,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    IsPlanned = transaction.IsPlanned,
                    Latitude = transaction.Latitude,
                    Longitue = transaction.Longitue,
                });
                return new CustomHttpResponse()
                {
                    message = "Transação feita com sucesso",
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
        public static async Task<CustomHttpResponse> GetTransactrionsPersistence()
        {
            TransactionsContext transactionsContext = new TransactionsContext();
            try
            {
                string query = "select * from public.transactions";
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
        public static async Task<CustomHttpResponse> GetTransactionSlidingWindowPersistence()
        {
            //TODO
            return new CustomHttpResponse()
            {

            };
        }
        public static async Task<CustomHttpResponse> GetTrasnactionsIntervalPersistence(CreateTransaction transaction)
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
        public static async Task<CustomHttpResponse> UpdateTrasnactionsPersistence(CreateTransaction transaction)
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
        public static async Task<CustomHttpResponse> DeleteTrasnactionsPersistence(CreateTransaction transaction)
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
        public static async Task<CustomHttpResponse> AddCategoriesPersistence (CreateCategories category)
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
                    message = "Categoria adicionada com sucesso",
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
                if(categoryExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "A categoria não existe",
                        status = 400
                    };
                }
                categoryExist.Name = name.Name;
                transactionsContext.Categories.Update(categoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Categoria atualizada com sucesso",
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
                        message = "A categoria não existe",
                        status = 400
                    };
                }
                transactionsContext.Categories.Remove(categoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Categoria removida com sucesso",
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
                    message = "Subcategoria adicionada com sucesso",
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
                        message = "A subcategoria não existe",
                        status = 400
                    };
                }
                subcategoryExist.Name = name.Name;
                transactionsContext.Subcategories.Update(subcategoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Subategoria atualizada com sucesso",
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
                        message = "A subcategoria não existe",
                        status = 400
                    };
                }
                transactionsContext.Subcategories.Remove(subcategoryExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Subcategoria removida com sucesso",
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
                    message = "Recorrente adicionado com sucesso",
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
                        message = "Recorrente não existe",
                        status = 400
                    };
                }
                transactionsContext.Reocurrings.Update(new Reocurring
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
                    message = "Recorrente atualizado com sucesso",
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
                        message = "A recorrente não existe",
                        status = 400
                    };
                }
                transactionsContext.Reocurrings.Remove(reocurringExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Recorrente removido com sucesso",
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
                    message = "Grupo de transação adicionado com sucesso",
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
                        message = "Grupo de transação não existe",
                        status = 400
                    };
                }
                transactionsContext.TransactionGroups.Update(new TransactionGroup
                {
                    Description = transactionGroup.Description,
                    StartDate = transactionGroup.StartDate,
                    EndDate = transactionGroup.EndDate,
                    PlannedAmount = transactionGroup.PlannedAmount
                });
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Grupo de transação com sucesso",
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
                        message = "O Grupo de transação não existe",
                        status = 400
                    };
                }
                transactionsContext.TransactionGroups.Remove(transactionGroupExist);
                await transactionsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Grupo de transação removido com sucesso",
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
}
