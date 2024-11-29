namespace BudgifyAPI.Accounts.CA.Entities.Requests
{
    public class CreateUser
    {
        public Guid? IdUserGroup { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public int Genre { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsManager { get; set; }

        public bool AllowWalletWatch { get; set; }
    }
}

