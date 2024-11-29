using BudgifyAPI.Accounts.CA.Entities;

namespace BudgifyAPI.Accounts.CA.UsesCases
{
    public class AccountsInteractorEF
    {
        public static async Task<CustomHttpResponse> GetGroupsById(Func<Guid, Task<CustomHttpResponse>> GetGroupsByIdPersistence, Guid userGroupId)
        {
            return await GetGroupsByIdPersistence(userGroupId);
        }
    }
}
