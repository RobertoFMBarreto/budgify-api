namespace BudgifyAPI.Transactions.Entities.Request;

public class WalletRequest
{
    public int status { get;  set; }
    public string? message { get; set; }
    public List<WalletRequestItem> data { get;  set; }
}