using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.UsesCases;
using Microsoft.AspNetCore.Http.HttpResults;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
namespace BudgifyAPI.Accounts.CA.Controllers
{
    public static class AccountsRoute
    {
        public static WebApplication SetRoutes(WebApplication application, string baseRoute)
        {
            application.MapPost($"{baseRoute}/group", async ([FromBody] RequestName name) => {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.AddUserGroup(AccountsPersistence.AddUserGroupPersistence, name);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }

            });

            application.MapPut($"{baseRoute}/group/{{userGroupId}}", async (Guid userGroupId, [FromBody] RequestName name) => {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.UpdateUserGroup(AccountsPersistence.UpdateUserGroupPersistence, userGroupId, name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/group/{{userGroupId}}", async (Guid userGroupId) => {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.DeleteUserGroup(AccountsPersistence.DeleteUserGroupPersistence, userGroupId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/group/{{userGroupId}}", async (Guid userGroupId) => {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.GetGroupsById(AccountsPersistence.GetGroupsByIdPersistence, userGroupId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/user-group/{{userId}}", async (Guid userId, [FromBody] CreateUser user) =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.AddUserToUserGroup(AccountsPersistence.AddUserToUserGroupPersistence, user, userId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/user-group/{{userId}}", async (Guid userId) => {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.DeleteUserFromUserGroup(AccountsPersistence.DeleteUserFromUserGroupPersistence, userId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/manager-group", async ([FromBody] User user) =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.AddManagerToUserGroup(AccountsPersistence.AddManagerToUserGroupPersistence, user);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            
            application.MapPost($"{baseRoute}/user", async ([FromBody] CreateUser user) =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.AddUser(AccountsPersistence.AddUserPersistence, user);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/user/{{userId}}", async (Guid userId, [FromBody] CreateUser user) =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.UpdateUser(AccountsPersistence.UpdateUserPersistence, userId, user);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/user/{{userId}}", async (Guid userId) =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.DeleteUser(AccountsPersistence.DeleteUserPersistence, userId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/user/{{userId}}", async (Guid userId) =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.ActiveUser(AccountsPersistence.ActiveUserPersistence, userId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/user", async () =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.GetUsers(AccountsPersistence.GetUsersPersistence);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/user/{{userId}}", async (Guid userId) =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.GetUserById(AccountsPersistence.GetUserByIdPersistence, userId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            return application;
        }
    }
}
