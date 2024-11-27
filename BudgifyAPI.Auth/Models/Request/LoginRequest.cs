using BudgifyAPI.Auth.Models.Request.Interfaces;

namespace BudgifyAPI.Auth.Models.Request;

public class LoginRequest:ILoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}