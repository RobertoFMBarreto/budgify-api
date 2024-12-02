using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;

namespace BudgifyAPI.Transactions.UseCases
{
    public class TransactionsInteractorEF
    {
        public static async Task<CustomHttpResponse> AddTransaction(Func<CreateTransaction, Task<CustomHttpResponse>> AddTransactionPersistence, CreateTransaction transaction)
        {
            return await AddTransactionPersistence(transaction);
        }
        public static async Task<CustomHttpResponse> GetTransactrions(Func<Task<CustomHttpResponse>> GetTransactrionsPersistence)
        {
            return await GetTransactrionsPersistence();
        }
        public static async Task<CustomHttpResponse> GetTransactionSlidingWindow(Func<Task<CustomHttpResponse>> GetTransactionSlidingWindowPersistence)
        {
            return await GetTransactionSlidingWindowPersistence();
        }
        public static async Task<CustomHttpResponse> GetTrasnactionsInterval(Func<CreateTransaction, Task<CustomHttpResponse>> GetTrasnactionsIntervalPersistence, CreateTransaction transaction)
        {
            return await GetTrasnactionsIntervalPersistence(transaction);
        }
        public static async Task<CustomHttpResponse> UpdateTrasnactions(Func<CreateTransaction, Task<CustomHttpResponse>> UpdateTrasnactionsPersistence, CreateTransaction transaction)
        {
            return await UpdateTrasnactionsPersistence(transaction);
        }
        public static async Task<CustomHttpResponse> DeleteTrasnactions(Func<Task<CustomHttpResponse>> DeleteTrasnactionsPersistence)
        {
            return await DeleteTrasnactionsPersistence();
        }
        public static async Task<CustomHttpResponse> GetCategories(Func<Task<CustomHttpResponse>> GetCategoriesPersistence)
        {
            return await GetCategoriesPersistence();
        }
        public static async Task<CustomHttpResponse> AddCategories(Func<CreateCategories, Task<CustomHttpResponse>> AddCategoriesPersistence, CreateCategories categories)
        {
            return await AddCategoriesPersistence(categories);
        }
        public static async Task<CustomHttpResponse> UpdateCategories(Func<Guid , RequestName, Task<CustomHttpResponse>> UpdateCategoriesPersistence, Guid categoryId, RequestName name)
        {
            return await UpdateCategoriesPersistence(categoryId, name);
        }
        public static async Task<CustomHttpResponse> DeleteCategory(Func<Guid, Task<CustomHttpResponse>> DeleteCategoryPersistence, Guid categoryId)
        {
            return await DeleteCategoryPersistence(categoryId);
        }
        public static async Task<CustomHttpResponse> AddSubcategories (Func<CreateSubcategory, Task<CustomHttpResponse>> AddSubcategoriesPersistence, CreateSubcategory subcategory)
        {
            return await AddSubcategoriesPersistence(subcategory);
        }
        public static async Task<CustomHttpResponse> UpdateSubcategories(Func<Guid, RequestName, Task<CustomHttpResponse>> UpdateSubcategoriesPersistence, Guid subcategoryId, RequestName name)
        {
            return await UpdateSubcategoriesPersistence(subcategoryId, name);
        }
        public static async Task<CustomHttpResponse> DeleteSubcategory(Func<Guid, Task<CustomHttpResponse>> DeleteSubcategoryPersistence, Guid subcategoryId)
        {
            return await DeleteSubcategoryPersistence(subcategoryId);
        }

        public static async Task<CustomHttpResponse> AddReocurring(Func<CreateReocurring,Task<CustomHttpResponse>> AddReocurringPersistence, CreateReocurring reocurring)
        {
            return await AddReocurringPersistence(reocurring);
        }

        public static async Task<CustomHttpResponse> UpdateReocurring(Func<Guid, CreateReocurring, Task<CustomHttpResponse>> UpdateReocurringPersistence, Guid reocurringId, CreateReocurring reocurring)
        {
            return await UpdateReocurringPersistence(reocurringId, reocurring);
        }

        public static async Task<CustomHttpResponse> DeleteReocurring(Func<Guid, Task<CustomHttpResponse>> DeleteReocurringPersistence, Guid reocurringId)
        {
            return await DeleteReocurringPersistence(reocurringId);
        }

        public static async Task<CustomHttpResponse> AddTransactionGroup(Func<CreateTransactionGroup, Task<CustomHttpResponse>> AddTransactionGroupPersistence, CreateTransactionGroup transactionGroup)
        {
            return await AddTransactionGroupPersistence(transactionGroup);
        }

        public static async Task<CustomHttpResponse> UpdateTransactionGroup(Func<Guid, CreateTransactionGroup, Task<CustomHttpResponse>> UpdateTransactionGroupPersistence, Guid transacationGroupId, CreateTransactionGroup transactionGroup)
        {
            return await UpdateTransactionGroupPersistence(transacationGroupId, transactionGroup);
        }
        public static async Task<CustomHttpResponse> DeleteTransactionGroup(Func<Guid, Task<CustomHttpResponse>> DeleteTransactionGroupPersistence, Guid transactionGroupID)
        {
            return await DeleteTransactionGroupPersistence(transactionGroupID);
        }

        public static async Task<CustomHttpResponse> GetTransactionsGroup(Func<Task<CustomHttpResponse>> GetTransactionsGroupPersistence)
        {
            return await GetTransactionsGroupPersistence();
        }





    }
}
