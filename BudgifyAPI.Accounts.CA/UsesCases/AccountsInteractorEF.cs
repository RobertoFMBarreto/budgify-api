using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;

namespace BudgifyAPI.Accounts.CA.UsesCases
{
    public class AccountsInteractorEF
    {
        public static async Task<CustomHttpResponse> AddUserGroup(Func<RequestName,Guid, Task<CustomHttpResponse>> AddUserGroupPersistence, RequestName name, Guid userId)
        {
            return await AddUserGroupPersistence(name, userId);
        }
        public static async Task<CustomHttpResponse> UpdateUserGroup(Func<Guid, RequestName,Guid, Task <CustomHttpResponse>> UpdateUserGroupPersistence, Guid userGroupId, RequestName name, Guid idUser)
        {
            return await UpdateUserGroupPersistence(userGroupId, name, idUser);
        }
        public static async Task<CustomHttpResponse> DeleteUserGroup(Func<Guid,Guid, Task<CustomHttpResponse>> DeleteUserGroupPersistence, Guid userGroupId, Guid userId)
        {
            return await DeleteUserGroupPersistence(userGroupId,userId);
        }
        public static async Task<CustomHttpResponse> GetGroupsById(Func<Guid,Guid, Task<CustomHttpResponse>> GetGroupsByIdPersistence, Guid userGroupId, Guid userId)
        {
            return await GetGroupsByIdPersistence(userGroupId, userId);
        }
        public static async Task<CustomHttpResponse> GetUserGroup(Func<Guid,Task<CustomHttpResponse>> GetUserGroupPersistence, Guid userId)
        {
            return await GetUserGroupPersistence(userId);
        }
        public static async Task<CustomHttpResponse> AddUserToUserGroup(Func<Guid, Guid, Guid, Task<CustomHttpResponse>> AddUserToUserGroupPersistence, Guid addUserId,Guid userGroupId, Guid userId)
        {
            return await AddUserToUserGroupPersistence(addUserId,userGroupId, userId);
        }
        public static async Task<CustomHttpResponse> DeleteUserFromUserGroup(Func<Guid,Guid,Guid, Task<CustomHttpResponse>> DeleteUserFromUserGroupPersistence, Guid removeUserId,Guid idUserGroup, Guid userId)
        {
            return await DeleteUserFromUserGroupPersistence(removeUserId, idUserGroup, userId);
        }
        public static async Task<CustomHttpResponse> AddManagerToUserGroup(Func<Guid, Guid,Guid, Task<CustomHttpResponse>> AddManagerToUserGroupPersistence, Guid managerId,Guid idUserGroup, Guid userId)
        {
            return await AddManagerToUserGroupPersistence(managerId,idUserGroup, userId);
        }
       public static async Task<CustomHttpResponse> DeleteManagerToUserGroup(Func<Guid,Guid,Guid, Task<CustomHttpResponse>>DeleteManagerToUserGroupPersistence,Guid removeManagerId,Guid idUserGroup, Guid userId)
        {
            return await DeleteManagerToUserGroupPersistence(removeManagerId, idUserGroup, userId);   
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
