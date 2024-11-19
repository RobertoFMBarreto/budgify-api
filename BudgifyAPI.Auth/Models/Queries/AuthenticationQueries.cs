using BudgifyAPI.Auth.Models.DB;
using BudgifyAPI.Auth.Models.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Auth.Models.Queries;

public class AuthenticationQueries:IAuthenticationQueries
{
    private readonly UserContext _context;

    public AuthenticationQueries()
    {
        _context = new UserContext();
    }
    public async Task<Guid?> Login(string email, string password)
    {
       User? user = await _context.Users.Where(u=> u.Email == email && u.Password == password).FirstOrDefaultAsync();
        return user?.IdUser;
    
    
}

    public async Task<bool> Register(string email, string password, string name, DateOnly dateOfBirth, int genre)
    {
        await _context.Users.AddAsync(new User()
        {
            Email = email,
            Password = password,
            Name = name,
            DateOfBirth = dateOfBirth,
            Genre = genre
        });
        return await _context.SaveChangesAsync() > 0;
    }
}