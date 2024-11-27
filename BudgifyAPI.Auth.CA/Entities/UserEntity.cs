namespace BudgifyAPI.Auth.CA.Entities;

public class UserEntity
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public int Genre { get; set; }

    public async Task<bool> Validate()
    {
        if (this.Name is null || this.Email is null || this.Password is null)
        {
            throw new Exception("missing fields");
            
        }

        return true;
    }
}