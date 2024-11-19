using BudgifyAPI.Auth.Models.Request.Interfaces;

namespace BudgifyAPI.Auth.Models.Request;

public class RegisterRequest : IRegisterRequest
{
    public string Email { get; set; }
    public string Pwd { get; set; }
    public string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int Genre { get; set; }
}