namespace BudgifyAPI.Transactions.Entities.Request
{
    public class CreateSubcategory
    {
        public Guid IdCategory { get; set; }

        public string Name { get; set; } = null!;

    }
}
