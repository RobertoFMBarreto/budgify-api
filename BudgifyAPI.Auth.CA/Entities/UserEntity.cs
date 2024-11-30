namespace BudgifyAPI.Auth.CA.Entities;

public class UserEntity
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public int Genre { get; set; }

    public async Task<CustomHttpResponse> Validate()
    {
        if (String.IsNullOrEmpty(Name)|| String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Password))
        {
            return new CustomHttpResponse()
            {
                status = 400,
                message = "missing fields",
            };
        }

        if (Genre is < 0 or > 2)
        {
            return new CustomHttpResponse()
            {
                status = 400,
                message = "missing fields",
            };
        }

        return  new CustomHttpResponse()
        {
            status = 200,
        };;
    }
}