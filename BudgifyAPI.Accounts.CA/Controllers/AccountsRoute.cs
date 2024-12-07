using System.Text;
using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.UsesCases;
using Microsoft.AspNetCore.Http.HttpResults;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using BudgifyAPI.Auth.CA.Entities;

namespace BudgifyAPI.Accounts.CA.Controllers
{
    public static class AccountsRoute
    {
        public static WebApplication SetRoutes(WebApplication application, string baseRoute)
        {
            application.MapPost($"{baseRoute}/user-group", async (HttpRequest req, [FromBody] RequestName name) => {
                try
                {
                    var received_uid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {received_uid}");
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp = await AccountsInteractorEF.AddUserGroup(AccountsPersistence.AddUserGroupPersistence, name, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }

            });
            application.MapPut($"{baseRoute}/user-group/admin/{{userGroupId}}", async (HttpRequest req, Guid userGroupId, [FromBody] RequestName name) => {
                try
                {
                    var received_uid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {received_uid}");
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp = await AccountsInteractorEF.UpdateUserGroup(AccountsPersistence.UpdateUserGroupPersistence, userGroupId, name, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/user-group/admin/{{userGroupId}}", async (HttpRequest req, Guid userGroupId) => {
                try
                {
                    var received_uid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {received_uid}");
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp = await AccountsInteractorEF.DeleteUserGroup(AccountsPersistence.DeleteUserGroupPersistence, userGroupId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/user-group/{{userGroupId}}", async (HttpRequest req, Guid userGroupId) => {
                try
                {
                    var received_uid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {received_uid}");
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp = await AccountsInteractorEF.GetGroupsById(AccountsPersistence.GetGroupsByIdPersistence, userGroupId, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/user-group", async (HttpRequest req) => {
                try
                {
                    var received_uid = req.Headers["X-User-Id"];
                    Console.WriteLine($"Received uid: {received_uid}");
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Bad Request",
                        };
                    }

                    var uid = CustomEncryptor.DecryptString(
                        Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));

                    CustomHttpResponse resp = await AccountsInteractorEF.GetUserGroup(AccountsPersistence.GetUserGroupPersistence,Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/user-group/manager/{{idUserGroup}}/users/{{addUserId}}", async (HttpRequest req,Guid idUserGroup, Guid addUserId) =>
            {
                

                try
                {
                    
                    var received_uid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.AddUserToUserGroup(AccountsPersistence.AddUserToUserGroupPersistence, addUserId,idUserGroup, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/user-group/manager/{{idUserGroup}}/users/{{addUserId}}", async (HttpRequest req, Guid idUserGroup, Guid removeUserId) => {
                try
                {
                    var received_uid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.DeleteUserFromUserGroup(AccountsPersistence.DeleteUserFromUserGroupPersistence,removeUserId,idUserGroup,Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPost($"{baseRoute}/user-group/admin/{{idUserGroup}}/manager/{{addUserId}}", async (HttpRequest req, Guid idUserGroup, Guid addManagerId) =>
            {
                try
                {
                    var received_uid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.AddManagerToUserGroup(AccountsPersistence.AddManagerToUserGroupPersistence, addManagerId, idUserGroup, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/user-group/admin/{{idUserGroup}}/manager/{{addUserId}}", async (HttpRequest req, Guid idUserGroup, Guid removeManagerId) =>
            {
                try
                {
                    var received_uid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.DeleteManagerToUserGroup(AccountsPersistence.DeleteManagerToUserGroupPersistence, removeManagerId, idUserGroup, Guid.Parse(uid));
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
            application.MapPut($"{baseRoute}/user", async (HttpRequest req, [FromBody] CreateUser user) =>
            {
                try
                {
                    var received_uid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.UpdateUser(AccountsPersistence.UpdateUserPersistence, Guid.Parse(uid), user);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapDelete($"{baseRoute}/admin/user", async (HttpRequest req) =>
            {
                try
                {
                    var received_uid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.DeleteUser(AccountsPersistence.DeleteUserPersistence, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapPut($"{baseRoute}/admin/user", async (HttpRequest req) =>
            {
                try
                {
                    var received_uid  = req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.ActiveUser(AccountsPersistence.ActiveUserPersistence, Guid.Parse(uid));
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            application.MapGet($"{baseRoute}/admin/user", async () =>
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
            application.MapGet($"{baseRoute}/admin/user/{{IdUser}}", async (HttpRequest req, Guid IdUser) =>
            {
                try
                {
                    var received_uid  =req.Headers["X-User-Id"];
                    if (string.IsNullOrEmpty(received_uid))
                    {
                        return new CustomHttpResponse()
                        {
                            status = 400,
                            message = "Missing token",
                        };
                
                    }
                    var uid = CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(received_uid)));
                    CustomHttpResponse resp = await AccountsInteractorEF.GetUserById(AccountsPersistence.GetUserByIdPersistence, IdUser);
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
