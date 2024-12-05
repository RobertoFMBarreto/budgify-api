using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;

namespace BudgifyAPI.Accounts.CA.UsesCases
{
    public class AccountsInteractorEF
    {
        public static async Task<CustomHttpResponse> AddUserGroup(Func<RequestName, Task<CustomHttpResponse>> AddUserGroupPersistence, RequestName name)
        {
            return await AddUserGroupPersistence(name);
        }
        public static async Task<CustomHttpResponse> UpdateUserGroup(Func<Guid, RequestName, Task <CustomHttpResponse>> UpdateUserGroupPersistence, Guid userGroupId, RequestName name)
        {
            return await UpdateUserGroupPersistence(userGroupId, name);
        }
        public static async Task<CustomHttpResponse> DeleteUserGroup(Func<Guid, Task<CustomHttpResponse>> DeleteUserGroupPersistence, Guid userGroupId)
        {
            return await DeleteUserGroupPersistence(userGroupId);
        }
        public static async Task<CustomHttpResponse> GetGroupsById(Func<Guid, Task<CustomHttpResponse>> GetGroupsByIdPersistence, Guid userGroupId)
        {
            return await GetGroupsByIdPersistence(userGroupId);
        }
        public static async Task<CustomHttpResponse> AddUserToUserGroup(Func<CreateUser, Guid, Task<CustomHttpResponse>> AddUserToUserGroupPersistence, CreateUser user, Guid userId)
        {
            return await AddUserToUserGroupPersistence(user, userId);
        }
        public static async Task<CustomHttpResponse> DeleteUserFromUserGroup(Func<Guid, Task<CustomHttpResponse>> DeleteUserFromUserGroupPersistence, Guid userId)
        {
            return await DeleteUserFromUserGroupPersistence(userId);
        }
        public static async Task<CustomHttpResponse> AddManagerToUserGroup(Func<User, Guid, Task<CustomHttpResponse>> AddManagerToUserGroupPersistence, User user, Guid userId)
        {
            return await AddManagerToUserGroupPersistence(user, userId);
        }
       public static async Task<CustomHttpResponse> DeleteManagerToUserGroup(Func<Guid, Task<CustomHttpResponse>>DeleteManagerToUserGroupPersistence,Guid userId)
        {
            return await DeleteManagerToUserGroupPersistence(userId);   
        }
        public static async Task<CustomHttpResponse> AddUser(Func<CreateUser, Task<CustomHttpResponse>> AddUserPersistence, CreateUser user)
        {
            return await AddUserPersistence(user);
        }
        public static async Task<CustomHttpResponse> UpdateUser(Func<Guid, CreateUser, Task<CustomHttpResponse>> UpdateUserPersistence, Guid userId, CreateUser user)
        {
            return await UpdateUserPersistence(userId, user);
        }
        public static async Task<CustomHttpResponse> DeleteUser(Func<Guid, Task<CustomHttpResponse>> DeleteUserPersistence, Guid userId)
        {
            return await DeleteUserPersistence(userId);
        }
        public static async Task<CustomHttpResponse> ActiveUser(Func<Guid, Task<CustomHttpResponse>> ActiveUserPersistence, Guid userId)
        {
            return await ActiveUserPersistence(userId);
        }
        public static async Task<CustomHttpResponse> GetUsers(Func<Task<CustomHttpResponse>> GetUsersPersistence)
        {
            return await GetUsersPersistence();
        }
        public static async Task<CustomHttpResponse> GetUserById(Func<Guid, Task<CustomHttpResponse>> GetUserByIdPersistence, Guid userId)
        {
            return await GetUserByIdPersistence(userId);
        }
    }
}
