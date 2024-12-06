namespace BudgifyAPI.Wallets.CA.Entities.Responses.GoCardless;

public class BankIndentification
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Bic { get; set; }
    public string TransactionTotalDays { get; set; }
    public List<string> Countries { get; set; }
    public string Logo { get; set; }
    public string MaxAccessValidForDays { get; set; }
}