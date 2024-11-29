namespace BudgifyAPI.Accounts.CA.Entities
{
    public class CustomHttpResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public object Data{ get; set; }
    }
}
