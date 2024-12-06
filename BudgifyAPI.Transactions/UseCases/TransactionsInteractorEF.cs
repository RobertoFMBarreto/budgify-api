using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;

namespace BudgifyAPI.Transactions.UseCases
{
    public class TransactionsInteractorEF
    {
        public static async Task<CustomHttpResponse> AddTransaction(
            Func<CreateTransaction, Guid, Task<CustomHttpResponse>> AddTransactionPersistence,
            CreateTransaction transaction, Guid userId)
        {
            return await AddTransactionPersistence(transaction, userId);
        }

        public static async Task<CustomHttpResponse> GetTrasnactionsInterval(
            Func<Task<CustomHttpResponse>> GetTrasnactionsIntervalPersistence)
        {
            return await GetTrasnactionsIntervalPersistence();
        }

        public static async Task<CustomHttpResponse> GetTransactions(
            Func<Guid, Task<CustomHttpResponse>> GetTransactionsPersistence, Guid uid)
        {
            return await GetTransactionsPersistence(uid);
        }

        public static async Task<CustomHttpResponse> GetTransactionSlidingWindow(
            Func<int, int, Guid, DateTime?, Task<CustomHttpResponse>> GetTransactionSlidingWindowPersistence,
            int limite, int cur_index, DateTime? date, Guid userId)
        {
            return await GetTransactionSlidingWindowPersistence(limite, cur_index, userId, date);
        }

        public static async Task<CustomHttpResponse> GetTransactionNoSlidingWindow(
            Func<DateTime, DateTime, Guid, Task<CustomHttpResponse>> GetTransactionNoSlidingWindowPersistence,
            DateIntervalRequest dates, Guid userId)
        {
            return await GetTransactionNoSlidingWindowPersistence(dates.StartDate, dates.EndDate, userId);
        }

        public static async Task<CustomHttpResponse> UpdateTransactions(
            Func<Guid, Guid, CreateTransaction, Task<CustomHttpResponse>> UpdateTrasnactionsPersistence,
            Guid transactionId, CreateTransaction transaction, Guid userId)
        {
            return await UpdateTrasnactionsPersistence(transactionId, userId, transaction);
        }

        public static async Task<CustomHttpResponse> DeleteTransactions(
            Func<Guid, Guid, Task<CustomHttpResponse>> DeleteTrasnactionsPersistence, Guid transactionId, Guid userId)
        {
            return await DeleteTrasnactionsPersistence(transactionId, userId);
        }

        public static async Task<CustomHttpResponse> GetCategories(
            Func<Guid, Task<CustomHttpResponse>> GetCategoriesPersistence, Guid userId)
        {
            return await GetCategoriesPersistence(userId);
        }

        public static async Task<CustomHttpResponse> AddCategories(
            Func<RequestName, Guid, Task<CustomHttpResponse>> AddCategoriesPersistence, RequestName categories,
            Guid userId)
        {
            return await AddCategoriesPersistence(categories, userId);
        }

        public static async Task<CustomHttpResponse> UpdateCategories(
            Func<Guid, RequestName, Guid, Task<CustomHttpResponse>> UpdateCategoriesPersistence, Guid categoryId,
            RequestName name, Guid userId)
        {
            return await UpdateCategoriesPersistence(categoryId, name, userId);
        }

        public static async Task<CustomHttpResponse> DeleteCategory(
            Func<Guid, Guid, Task<CustomHttpResponse>> DeleteCategoryPersistence, Guid categoryId, Guid userId)
        {
            return await DeleteCategoryPersistence(categoryId, userId);
        }

        public static async Task<CustomHttpResponse> AddSubcategories(
            Func<CreateSubcategory, Guid, Task<CustomHttpResponse>> AddSubcategoriesPersistence,
            CreateSubcategory subcategory, Guid userId)
        {
            return await AddSubcategoriesPersistence(subcategory, userId);
        }

        public static async Task<CustomHttpResponse> UpdateSubcategories(
            Func<Guid, RequestName, Guid, Task<CustomHttpResponse>> UpdateSubcategoriesPersistence, Guid subcategoryId,
            RequestName name, Guid userId)
        {
            return await UpdateSubcategoriesPersistence(subcategoryId, name, userId);
        }

        public static async Task<CustomHttpResponse> DeleteSubcategory(
            Func<Guid, Guid, Task<CustomHttpResponse>> DeleteSubcategoryPersistence, Guid subcategoryId, Guid userId)
        {
            return await DeleteSubcategoryPersistence(subcategoryId, userId);
        }

        public static async Task<CustomHttpResponse> AddReocurring(
            Func<CreateReocurring, Guid, Task<CustomHttpResponse>> AddReocurringPersistence,
            CreateReocurring reocurring, Guid userId)
        {
            return await AddReocurringPersistence(reocurring, userId);
        }

        public static async Task<CustomHttpResponse> UpdateReocurring(
            Func<Guid, CreateReocurring, Guid, Task<CustomHttpResponse>> UpdateReocurringPersistence, Guid reocurringId,
            CreateReocurring reocurring, Guid userId)
        {
            return await UpdateReocurringPersistence(reocurringId, reocurring, userId);
        }

        public static async Task<CustomHttpResponse> DeleteReocurring(
            Func<Guid, Guid, Task<CustomHttpResponse>> DeleteReocurringPersistence, Guid reocurringId, Guid userId)
        {
            return await DeleteReocurringPersistence(reocurringId, userId);
        }

        public static async Task<CustomHttpResponse> AddTransactionGroup(
            Func<CreateTransactionGroup, Guid, Task<CustomHttpResponse>> AddTransactionGroupPersistence,
            CreateTransactionGroup transactionGroup, Guid userId)
        {
            return await AddTransactionGroupPersistence(transactionGroup, userId);
        }

        public static async Task<CustomHttpResponse> UpdateTransactionGroup(
            Func<Guid, CreateTransactionGroup, Guid, Task<CustomHttpResponse>> UpdateTransactionGroupPersistence,
            Guid transacationGroupId, CreateTransactionGroup transactionGroup, Guid userId)
        {
            return await UpdateTransactionGroupPersistence(transacationGroupId, transactionGroup, userId);
        }

        public static async Task<CustomHttpResponse> DeleteTransactionGroup(
            Func<Guid,Guid, Task<CustomHttpResponse>> DeleteTransactionGroupPersistence, Guid transactionGroupID, Guid userId)
        {
            return await DeleteTransactionGroupPersistence(transactionGroupID, userId);
        }

        public static async Task<CustomHttpResponse> GetTransactionsGroup(
            Func<Guid,Task<CustomHttpResponse>> GetTransactionsGroupPersistence, Guid userId)
        {
            return await GetTransactionsGroupPersistence(userId);
        }

        public static async Task<CustomHttpResponse> GetGroupTransactionStats(
            Func<Guid, Task<CustomHttpResponse>> GetGroupTransactionStatsPersistence, Guid transactionGroupId)
        {
            return await GetGroupTransactionStatsPersistence(transactionGroupId);
        }
    }
}