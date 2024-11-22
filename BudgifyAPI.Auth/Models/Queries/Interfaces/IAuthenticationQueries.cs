using BudgifyAPI.Auth.Models.DB;

namespace BudgifyAPI.Auth.Models.Queries.Interfaces;

public interface IAuthenticationQueries
{
    public Task<Guid?>  Login(string username, string password);
    
    public Task<bool> Register(string email, string password, string name, DateOnly dateOfBirth, int genre);
    public Task<List<User>> GetUsers();
}