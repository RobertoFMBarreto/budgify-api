using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Transactions.Entities.Request;
using BudgifyAPI.Transactions.Framework.EntityFramework.Models;

namespace BudgifyAPI.Transactions.UseCases
{
    public class TransactionsInteractorEf
    {
        public static async Task<CustomHttpResponse> AddTransaction(
            Func<CreateTransaction, Guid, Task<CustomHttpResponse>> addTransactionPersistence,
            CreateTransaction transaction, Guid userId)
        {
            return await addTransactionPersistence(transaction, userId);
        }

        public static async Task<CustomHttpResponse> GetTrasnactionsInterval(
            Func<Task<CustomHttpResponse>> getTrasnactionsIntervalPersistence)
        {
            return await getTrasnactionsIntervalPersistence();
        }

        public static async Task<CustomHttpResponse> GetTransactions(
            Func<Guid, Task<CustomHttpResponse>> getTransactionsPersistence, Guid uid)
        {
            return await getTransactionsPersistence(uid);
        }

        public static async Task<CustomHttpResponse> GetTransactionSlidingWindow(
            Func<int, int, Guid, DateTime?, Task<CustomHttpResponse>> getTransactionSlidingWindowPersistence,
            int limite, int curIndex, DateTime? date, Guid userId)
        {
            return await getTransactionSlidingWindowPersistence(limite, curIndex, userId, date);
        }

        public static async Task<CustomHttpResponse> GetTransactionNoSlidingWindow(
            Func<DateTime, DateTime, Guid, Task<CustomHttpResponse>> getTransactionNoSlidingWindowPersistence,
            DateIntervalRequest dates, Guid userId)
        {
            return await getTransactionNoSlidingWindowPersistence(dates.StartDate, dates.EndDate, userId);
        }

        public static async Task<CustomHttpResponse> UpdateTransactions(
            Func<Guid, Guid, CreateTransaction, Task<CustomHttpResponse>> updateTrasnactionsPersistence,
            Guid transactionId, CreateTransaction transaction, Guid userId)
        {
            return await updateTrasnactionsPersistence(transactionId, userId, transaction);
        }

        public static async Task<CustomHttpResponse> DeleteTransactions(
            Func<Guid, Guid, Task<CustomHttpResponse>> deleteTrasnactionsPersistence, Guid transactionId, Guid userId)
        {
            return await deleteTrasnactionsPersistence(transactionId, userId);
        }

        public static async Task<CustomHttpResponse> GetCategories(
            Func<Guid, Task<CustomHttpResponse>> getCategoriesPersistence, Guid userId)
        {
            return await getCategoriesPersistence(userId);
        }

        public static async Task<CustomHttpResponse> AddCategories(
            Func<RequestName, Guid, Task<CustomHttpResponse>> addCategoriesPersistence, RequestName categories,
            Guid userId)
        {
            return await addCategoriesPersistence(categories, userId);
        }

        public static async Task<CustomHttpResponse> UpdateCategories(
            Func<Guid, RequestName, Guid, Task<CustomHttpResponse>> updateCategoriesPersistence, Guid categoryId,
            RequestName name, Guid userId)
        {
            return await updateCategoriesPersistence(categoryId, name, userId);
        }

        public static async Task<CustomHttpResponse> DeleteCategory(
            Func<Guid, Guid, Task<CustomHttpResponse>> deleteCategoryPersistence, Guid categoryId, Guid userId)
        {
            return await deleteCategoryPersistence(categoryId, userId);
        }

        public static async Task<CustomHttpResponse> GetSubcategories(
            Func<Guid, Task<CustomHttpResponse>> getSubcategoriesPersistence, Guid userId)
        {
            return await getSubcategoriesPersistence(userId);
        }
        public static async Task<CustomHttpResponse> AddSubcategories(
            Func<CreateSubcategory, Guid, Task<CustomHttpResponse>> addSubcategoriesPersistence,
            CreateSubcategory subcategory, Guid userId)
        {
            return await addSubcategoriesPersistence(subcategory, userId);
        }

        public static async Task<CustomHttpResponse> UpdateSubcategories(
            Func<Guid, RequestName, Guid, Task<CustomHttpResponse>> updateSubcategoriesPersistence, Guid subcategoryId,
            RequestName name, Guid userId)
        {
            return await updateSubcategoriesPersistence(subcategoryId, name, userId);
        }

        public static async Task<CustomHttpResponse> DeleteSubcategory(
            Func<Guid, Guid, Task<CustomHttpResponse>> deleteSubcategoryPersistence, Guid subcategoryId, Guid userId)
        {
            return await deleteSubcategoryPersistence(subcategoryId, userId);
        }
        public static async Task<CustomHttpResponse> GetReocurring(
            Func <Guid, Task<CustomHttpResponse>> getReocurringPersistence, Guid uiserId)
        {
            return await getReocurringPersistence(uiserId);
        }
        public static async Task<CustomHttpResponse> AddReocurring(
            Func<CreateReocurring, Guid, Task<CustomHttpResponse>> addReocurringPersistence,
            CreateReocurring reocurring, Guid userId)
        {
            return await addReocurringPersistence(reocurring, userId);
        }

        public static async Task<CustomHttpResponse> UpdateReocurring(
            Func<Guid, CreateReocurring, Guid, Task<CustomHttpResponse>> updateReocurringPersistence, Guid reocurringId,
            CreateReocurring reocurring, Guid userId)
        {
            return await updateReocurringPersistence(reocurringId, reocurring, userId);
        }

        public static async Task<CustomHttpResponse> DeleteReocurring(
            Func<Guid, Guid, Task<CustomHttpResponse>> deleteReocurringPersistence, Guid reocurringId, Guid userId)
        {
            return await deleteReocurringPersistence(reocurringId, userId);
        }
        public static async Task<CustomHttpResponse> GetTransactionGroup(
            Func<Guid, Task<CustomHttpResponse>>getTransactionGroupPersistence, Guid userId)
        {
            return await getTransactionGroupPersistence(userId);    
        }
        public static async Task<CustomHttpResponse> AddTransactionGroup(
            Func<CreateTransactionGroup, Guid, Task<CustomHttpResponse>> addTransactionGroupPersistence,
            CreateTransactionGroup transactionGroup, Guid userId)
        {
            return await addTransactionGroupPersistence(transactionGroup, userId);
        }

        public static async Task<CustomHttpResponse> UpdateTransactionGroup(
            Func<Guid, CreateTransactionGroup, Guid, Task<CustomHttpResponse>> updateTransactionGroupPersistence,
            Guid transacationGroupId, CreateTransactionGroup transactionGroup, Guid userId)
        {
            return await updateTransactionGroupPersistence(transacationGroupId, transactionGroup, userId);
        }

        public static async Task<CustomHttpResponse> DeleteTransactionGroup(
            Func<Guid,Guid, Task<CustomHttpResponse>> deleteTransactionGroupPersistence, Guid transactionGroupId, Guid userId)
        {
            return await deleteTransactionGroupPersistence(transactionGroupId, userId);
        }

        public static async Task<CustomHttpResponse> GetTransactionsGroup(
            Func<Guid,Task<CustomHttpResponse>> getTransactionsGroupPersistence, Guid userId)
        {
            return await getTransactionsGroupPersistence(userId);
        }

        public static async Task<CustomHttpResponse> GetGroupTransactionStats(
            Func<Guid, Task<CustomHttpResponse>> getGroupTransactionStatsPersistence, Guid transactionGroupId)
        {
            return await getGroupTransactionStatsPersistence(transactionGroupId);
        }

        public static async Task<CustomHttpResponse> ExportTransactions(
            Func<Guid, Guid, Task<CustomHttpResponse>> exportTransactionsPersistence, Guid userId, Guid walletId)
        {
            return await exportTransactionsPersistence(userId, walletId);
        }
    }
}