using BudgifyAPI.Transactions.Framework.EntityFramework.Models;

namespace BudgifyAPI.Transactions.Entities.Responses;

public class TransactionsResponse
{
    public List<Transaction> Transactions { get; set; }
    public List<TransactionGroup> TransactionGroups { get;  set; }
}