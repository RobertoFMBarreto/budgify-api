namespace BudgifyAPI.Auth.Helpers.Interfaces;

public interface IPasetoHelper
{
    public string GeneratePasetoToken(Guid userId);
}