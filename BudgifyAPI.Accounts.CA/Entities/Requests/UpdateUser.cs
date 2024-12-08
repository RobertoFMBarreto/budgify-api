namespace BudgifyAPI.Accounts.CA.Entities.Requests;

public class UpdateUser
{
    public Guid? IdUserGroup { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public int Genre { get; set; }

    public bool AllowWalletWatch { get; set; }
}