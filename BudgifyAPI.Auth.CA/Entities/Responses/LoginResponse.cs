namespace BudgifyAPI.Auth.CA.Entities.Responses;

public class LoginResponse
{
    public string Token { get;  set; }
    public string RefreshToken { get;  set; }
}