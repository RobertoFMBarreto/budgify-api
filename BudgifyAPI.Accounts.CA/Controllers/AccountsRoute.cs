﻿using System.Text;
using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.UsesCases;
using Microsoft.AspNetCore.Http.HttpResults;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;
using Microsoft.AspNetCore.Authorization;

namespace BudgifyAPI.Accounts.CA.Controllers
{
    public static class AccountsRoute
    {
        public static WebApplication SetRoutes(WebApplication application, string baseRoute)
        {
            application.MapPost($"{baseRoute}/user/user-group", async (HttpRequest req, [FromBody] RequestName name) => {
                try
                {
                    var receivedUid = req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp = await AccountsInteractorEf.AddUserGroup(AccountsPersistence.AddUserGroupPersistence, name, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }

            });
            application.MapPut($"{baseRoute}/admin/user-group/{{userGroupId}}", async (HttpRequest req, Guid userGroupId, [FromBody] RequestName name) => {
                try
                {
                    var receivedUid = req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp = await AccountsInteractorEf.UpdateUserGroup(AccountsPersistence.UpdateUserGroupPersistence, userGroupId, name, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/admin/user-group/{{userGroupId}}", async (HttpRequest req, Guid userGroupId) => {
                try
                {
                    var receivedUid = req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp = await AccountsInteractorEf.DeleteUserGroup(AccountsPersistence.DeleteUserGroupPersistence, userGroupId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/user/user-group/{{userGroupId}}", async (HttpRequest req, Guid userGroupId) => {
                try
                {
                    var receivedUid = req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp = await AccountsInteractorEf.GetGroupsById(AccountsPersistence.GetGroupsByIdPersistence, userGroupId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/user/user-group", async (HttpRequest req) => {
                try
                {
                    var receivedUid = req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));

                    CustomHttpResponse resp = await AccountsInteractorEf.GetUserGroup(AccountsPersistence.GetUserGroupPersistence,Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/manager/user-group/users/", async (HttpRequest req, [FromBody]RequestUid  addUserId) =>
            {
                

                try
                {
                    
                    var receivedUid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.AddUserToUserGroup(AccountsPersistence.AddUserToUserGroupPersistence, addUserId.UserId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/manager/user-group/users/", async (HttpRequest req, [FromBody]RequestUid removeUserId) => {
                try
                {
                    var receivedUid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.DeleteUserFromUserGroup(AccountsPersistence.DeleteUserFromUserGroupPersistence,removeUserId.UserId,Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/admin/user-group/manager/", async (HttpRequest req, [FromBody]RequestUid addManagerId) =>
            {
                try
                {
                    var receivedUid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.AddManagerToUserGroup(AccountsPersistence.AddManagerToUserGroupPersistence, addManagerId.UserId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/admin/user-group/manager/", async (HttpRequest req, [FromBody]RequestUid  removeManagerId) =>
            {
                try
                {
                    var receivedUid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.DeleteManagerToUserGroup(AccountsPersistence.DeleteManagerToUserGroupPersistence, removeManagerId.UserId, Guid.Parse(uid));
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
                    CustomHttpResponse resp = await AccountsInteractorEf.AddUser(AccountsPersistence.AddUserPersistence, user);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/new-password",[AllowAnonymous] async ([FromBody]NewPasswordRequest body)=>
            {
                try
                {
                    
                    CustomHttpResponse resp = await AccountsInteractorEf.NewPassword(AccountsPersistence.NewPasswordPersistence,body );
                    return resp;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/user", async (HttpRequest req, [FromBody] UpdateUser user) =>
            {
                try
                {
                    var receivedUid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.UpdateUser(AccountsPersistence.UpdateUserPersistence, Guid.Parse(uid), user);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/superadmin/user/{{userId}}", async (HttpRequest req, Guid userId) =>
            {
                try
                {
                    var receivedUid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.DeleteUser(AccountsPersistence.DeleteUserPersistence, userId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/superadmin/user/{{userId}}", async (HttpRequest req,Guid userId) =>
            {
                try
                {
                    var receivedUid  = req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.ActiveUser(AccountsPersistence.ActiveUserPersistence, userId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/superadmin/user", async () =>
            {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEf.GetUsers(AccountsPersistence.GetUsersPersistence);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/superadmin/user/{{idUser}}", async (HttpRequest req, Guid idUser) =>
            {
                try
                {
                    var receivedUid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(receivedUid))
                    {
                        return new CustomHttpResponse()
                        {
                            Status = 400,
                            Message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(receivedUid)));
                    CustomHttpResponse resp = await AccountsInteractorEf.GetUserById(AccountsPersistence.GetUserByIdPersistence, idUser);
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
