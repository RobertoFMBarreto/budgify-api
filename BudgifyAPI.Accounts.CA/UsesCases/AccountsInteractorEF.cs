using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using BudgifyAPI.Auth.CA.Entities.Requests;

namespace BudgifyAPI.Accounts.CA.UsesCases
{
    public class AccountsInteractorEf
    {
        public static async Task<CustomHttpResponse> AddUserGroup(Func<RequestName,Guid, Task<CustomHttpResponse>> addUserGroupPersistence, RequestName name, Guid userId)
        {
            return await addUserGroupPersistence(name, userId);
        }
        
        
        public static async Task<CustomHttpResponse> UpdateUserGroup(Func<Guid, RequestName,Guid, Task <CustomHttpResponse>> updateUserGroupPersistence, Guid userGroupId, RequestName name, Guid idUser)
        {
            return await updateUserGroupPersistence(userGroupId, name, idUser);
        }
        public static async Task<CustomHttpResponse> DeleteUserGroup(Func<Guid,Guid, Task<CustomHttpResponse>> deleteUserGroupPersistence, Guid userGroupId, Guid userId)
        {
            return await deleteUserGroupPersistence(userGroupId,userId);
        }
        public static async Task<CustomHttpResponse> GetGroupsById(Func<Guid,Guid, Task<CustomHttpResponse>> getGroupsByIdPersistence, Guid userGroupId, Guid userId)
        {
            return await getGroupsByIdPersistence(userGroupId, userId);
        }
        public static async Task<CustomHttpResponse> GetUserGroup(Func<Guid,Task<CustomHttpResponse>> getUserGroupPersistence, Guid userId)
        {
            return await getUserGroupPersistence(userId);
        }
        public static async Task<CustomHttpResponse> AddUserToUserGroup(Func<Guid, Guid, Task<CustomHttpResponse>> addUserToUserGroupPersistence, Guid addUserId, Guid userId)
        {
            return await addUserToUserGroupPersistence(addUserId, userId);
        }
        public static async Task<CustomHttpResponse> DeleteUserFromUserGroup(Func<Guid,Guid, Task<CustomHttpResponse>> deleteUserFromUserGroupPersistence, Guid removeUserId, Guid userId)
        {
            return await deleteUserFromUserGroupPersistence(removeUserId, userId);
        }
        public static async Task<CustomHttpResponse> AddManagerToUserGroup(Func<Guid,Guid, Task<CustomHttpResponse>> addManagerToUserGroupPersistence, Guid managerId, Guid userId)
        {
            return await addManagerToUserGroupPersistence(managerId, userId);
        }
       public static async Task<CustomHttpResponse> DeleteManagerToUserGroup(Func<Guid,Guid, Task<CustomHttpResponse>>deleteManagerToUserGroupPersistence,Guid removeManagerId, Guid userId)
        {
            return await deleteManagerToUserGroupPersistence(removeManagerId, userId);   
        }
        public static async Task<CustomHttpResponse> AddUser(Func<CreateUser, Task<CustomHttpResponse>> addUserPersistence, CreateUser user)
        {
            return await addUserPersistence(user);
        }

        public static async Task<CustomHttpResponse> NewPassword(
            Func<NewPasswordRequest, Task<CustomHttpResponse>> newPasswordPersistence,
            NewPasswordRequest newPasswordRequest)
        {
            return await newPasswordPersistence(newPasswordRequest);
        }
        public static async Task<CustomHttpResponse> UpdateUser(Func<Guid, UpdateUser, Task<CustomHttpResponse>> updateUserPersistence, Guid userId, UpdateUser user)
        {
            return await updateUserPersistence(userId, user);
        }
        public static async Task<CustomHttpResponse> DeleteUser(Func<Guid, Task<CustomHttpResponse>> deleteUserPersistence, Guid userId)
        {
            return await deleteUserPersistence(userId);
        }
        public static async Task<CustomHttpResponse> ActiveUser(Func<Guid, Task<CustomHttpResponse>> activeUserPersistence, Guid userId)
        {
            return await activeUserPersistence(userId);
        }
        public static async Task<CustomHttpResponse> GetUsers(Func<Task<CustomHttpResponse>> getUsersPersistence)
        {
            return await getUsersPersistence();
        }
        public static async Task<CustomHttpResponse> GetUserById(Func<Guid, Task<CustomHttpResponse>> getUserByIdPersistence, Guid userId)
        {
            return await getUserByIdPersistence(userId);
        }
        
    }
}
