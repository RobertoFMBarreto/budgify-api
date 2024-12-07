

namespace BudgifyAPI.Wallets.CA.Entities.Requests
{
    public class WalletRequests
    {
        public string wallet_name { get; set;}
    }
   public class DeleteWalletRequest
    {
        public Guid user_id { get; set; } 
        public Guid wallet_id { get; set;}
    }
    public class GetWalletRequest
    {
        public Guid user_id { get; set; } 
        public Guid wallet_id { get; set;}
    }

    public class EditWalletRequest
    {
        public Guid wallet_id { get; set; }
        public string wallet_name { get; set; }
        public float value { get; set; }
        
        public string? IdRequisition {get; set;}

        public int? AgreementDays { get; set; }
        public string? IdAccount { get; set; }

    }
}