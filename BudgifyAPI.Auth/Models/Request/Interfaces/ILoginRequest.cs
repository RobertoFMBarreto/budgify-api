namespace BudgifyAPI.Auth.Models.Request.Interfaces;

public interface ILoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}