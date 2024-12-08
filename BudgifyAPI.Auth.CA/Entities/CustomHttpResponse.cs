namespace BudgifyAPI.Auth.CA.Entities;

public class CustomHttpResponse
{
    public int Status { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}