using Microsoft.AspNetCore.Mvc;

namespace BudgifyAPI.Auth.Services.Interfaces;

public interface IAuthenticationServices
{
    public Task<string?> login(string email, string password);
    
    public Task<bool> register(string email, string password, string name, DateOnly dateOfBirth, int genre);
}