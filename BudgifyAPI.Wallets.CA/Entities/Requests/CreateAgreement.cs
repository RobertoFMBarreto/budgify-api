namespace BudgifyAPI.Wallets.CA.Entities.Requests;

public class CreateAgreement
{
    public string Intitution { get;  set; }
    public string MaxHistoricalDays { get;  set; }
    public string AccesValidForDays { get;  set; }
}