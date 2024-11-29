using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Entities.Requests;

namespace BudgifyAPI.Accounts.CA.UsesCases
{
    public class AccountsInteractorEF
    {
        public static async Task<CustomHttpResponse> AddUserGroup(Func<Guid, Task<CustomHttpResponse>> AddUserGroupPersistence, RequestName name)
        {
            return null;
            //return await AddUserGroupPersistence(name);
        }
        public static async Task<CustomHttpResponse> GetGroupsById(Func<Guid, Task<CustomHttpResponse>> GetGroupsByIdPersistence, Guid userGroupId)
        {
            return await GetGroupsByIdPersistence(userGroupId);
        }
    }
}
