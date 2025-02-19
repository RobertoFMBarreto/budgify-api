namespace BudgifyAPI.Auth.CA.Entities.Requests;

public class NewSessionTokenRequest
{
    public string RefreshToken { get;  set; }

    public bool Validate()
    {
        return string.IsNullOrWhiteSpace(RefreshToken);
    }
}