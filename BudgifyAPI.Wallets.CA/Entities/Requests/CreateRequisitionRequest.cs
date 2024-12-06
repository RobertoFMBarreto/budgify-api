namespace BudgifyAPI.Wallets.CA.Entities.Requests;

public class CreateRequisitionRequest
{
    public string Redirect { get; set; }
    public string InstitutionId { get; set; }
    public string Agreement { get; set; }
    public string UserLanguage { get; set; }
    
}