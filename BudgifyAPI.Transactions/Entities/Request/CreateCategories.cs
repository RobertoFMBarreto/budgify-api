namespace BudgifyAPI.Transactions.Entities.Request
{
    public class CreateCategories
    {
        public string Name { get; set; } = null!;

        public Guid? IdUser { get; set; }

    }
}
