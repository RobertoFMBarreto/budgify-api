namespace BudgifyAPI.Transactions.Entities.Request
{
    public class CreateTransactionGroup
    {
        public string Description { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public float PlannedAmount { get; set; }
    }
}
