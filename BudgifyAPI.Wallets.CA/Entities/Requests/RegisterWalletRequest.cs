

namespace BudgifyAPI.Wallets.CA.Entities.Requests
{
    public class RegisterWalletRequest
    {
        public Guid user_id { get; set; } 
        public string wallet_name { get; set;}
    }
}