namespace BudgifyAPI.Transactions.Entities.Request;

public class WalletRequestItem
{
    public string idWallet { get; set; }
    public string idUser { get; set; }
    public string name { get; set; }
    public string? idRequisition { get; set; }
    public int? agreementDays { get; set; }
    public string? idAccount { get; set; }
    public bool storeInCloud { get; set; }


}