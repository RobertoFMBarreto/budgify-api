

namespace BudgifyAPI.Wallets.CA.Entities.Requests
{
    public class RegisterWalletRequest
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
}