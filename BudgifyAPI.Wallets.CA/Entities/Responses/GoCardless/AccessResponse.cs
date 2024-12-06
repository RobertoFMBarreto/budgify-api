namespace BudgifyAPI.Wallets.CA.Entities.Responses.GoCardless;

public class AccessResponse
{
    public string Access { get; set; }
    public string AccessExpires { get; set; }
    public string Refresh { get; set; }
    public string RefreshExpires { get; set; }
}