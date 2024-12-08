

namespace BudgifyAPI.Wallets.CA.Entities.Requests
{
    public class WalletRequests
    {
        public string WalletName { get; set;}
    }
   public class DeleteWalletRequest
    {
        public Guid UserId { get; set; } 
        public Guid WalletId { get; set;}
    }
    public class GetWalletRequest
    {
        public Guid UserId { get; set; } 
        public Guid WalletId { get; set;}
    }

    public class EditWalletRequest
    {
        public Guid WalletId { get; set; }
        public string WalletName { get; set; }
        public float Value { get; set; }
        
        public string? IdRequisition {get; set;}

        public int? AgreementDays { get; set; }
        public string? IdAccount { get; set; }

    }
}