using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace BudgifyAPI.Accounts.CA.UsesCases
{
    public static class AccountsPersistence
    {
        public static async Task<CustomHttpResponse> AddUserGroupPersistence(RequestName name, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var user = await accountsContext.Users.Where(u=> u.IdUser == userId).FirstOrDefaultAsync();
                if (user.IdUserGroup != null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "User already has group",
                    };
                }

                UserGroup userGroup = new UserGroup()
                {
                    IdUserGroup = Guid.NewGuid(),
                    Name = name.name,
                };
                await accountsContext.UserGroups.AddAsync(userGroup);
                
                user.IdUserGroup = userGroup.IdUserGroup;
                user.IsAdmin = true;
                accountsContext.Users.Update(user);
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "User group added successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> UpdateUserGroupPersistence(Guid userGroupId, RequestName name, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            { 
                var userGroupExist = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (userGroupExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Group doesn't exist",
                        status = 400
                    };
                }
                
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                userGroupExist.Name = name.name;
                accountsContext.UserGroups.Update(userGroupExist);
                await accountsContext.SaveChangesAsync();

                return new CustomHttpResponse()
                {
                    message = "User group updated successfully",
                    status = 200
                };
                
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteUserGroupPersistence(Guid userGroupId, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userGroupExist = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (userGroupExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                
                string queryUserGroupNull = "update public.user set id_user_group = null where id_user_group = @IdUserGroup";
                accountsContext.Database.ExecuteSqlRaw(queryUserGroupNull, new NpgsqlParameter("@IdUserGroup", userGroupId));
                accountsContext.UserGroups.Remove(userGroupExist);
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "User group removed successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }

        }
        public static async Task<CustomHttpResponse> GetGroupsByIdPersistence(Guid userGroupId, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userGroupExist = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (userGroupExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                return new CustomHttpResponse()
                {
                    status = 200,
                    Data = userGroupExist,

                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    status = 500,
                    message = ex.Message
                };
            }
        }
        public static async Task<CustomHttpResponse> GetUserGroupPersistence(Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var user = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (user.IdUserGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 200,
                        message = "User doesn't have group",
                    };
                }
                var userGroups = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == user.IdUserGroup);
                
                return new CustomHttpResponse()
                {
                    status = 200,
                    Data = userGroups,

                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    status = 500,
                    message = ex.Message
                };
            }
        }
        public static async Task<CustomHttpResponse> AddUserToUserGroupPersistence(Guid addUserId,Guid userGroupId, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (existGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Group does not exist",
                        status = 400
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId &&
                    (x.IsAdmin == true || x.IsManager == true));
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == addUserId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }
                if (userExist.IdUserGroup != null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User is already in a user group",
                        status = 400
                    };
                }
                
                string query = "update public.user " +
                    $"set id_user_group = @IdUserGroup " +
                    $"where id_user = @IdUser";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUserGroup", userGroupId), new NpgsqlParameter("@IdUser", addUserId));
                return new CustomHttpResponse()
                {
                    message = "User added to a user group successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    status = 500,
                    message = ex.Message
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteUserFromUserGroupPersistence(Guid removeUserId,Guid idUserGroup, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == idUserGroup);
                if (existGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Group does not exist",
                        status = 400
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId &&
                    (x.IsAdmin == true || x.IsManager == true));
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == removeUserId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }
                if (userExist.IdUserGroup != existGroup.IdUserGroup)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User is in another group",
                        status = 400
                    };
                }
                string query = "update public.user " +
                    "set id_user_group = null " +
                    "where id_user = @IdUser ";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", userId));
                return new CustomHttpResponse()
                {
                    message = "User removed successfully",
                    status = 200
                };

            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddManagerToUserGroupPersistence(Guid managerId,Guid idUserGroup, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == idUserGroup);
                if (existGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == managerId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User group does not exist",
                        status = 400
                    };
                }
                if (userExist.IdUserGroup != existGroup.IdUserGroup)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User is already in a user group",
                        status = 400
                    };
                }
                string query = "update public.user " +
                  $"set id_user_group = @IdUserGroup " +
                  $"set is_Manager = true, " +
                  $"where id_user = @IdUser";

                await accountsContext.Database.ExecuteSqlRawAsync(query,
                    new NpgsqlParameter("@IdUserGroup", idUserGroup),
                    new NpgsqlParameter("@IdUser", managerId));

                return new CustomHttpResponse()
                {
                    message = "Manager added successfully",
                    status = 200
                }; 
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteManagerToUserGroupPersistence(Guid removeManagerId,Guid idUserGroup, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == removeManagerId);
                if (userExist == null)
                    return new CustomHttpResponse()
                    {
                        message = "Manager does not exist",
                        status = 400
                    };
                
                var existGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == idUserGroup);
                if (existGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "Group doesn't exist",
                    };
                }
                
                if (userExist.IdUserGroup != existGroup.IdUserGroup)
                {
                    return new CustomHttpResponse()
                    {
                        status = 400,
                        message = "User is in another group",
                    };
                }
                
                
                string query = "update public.user " +
                "set is_manager = false " +
                "where id_user = @IdUser ";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", removeManagerId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Manager removed successfully",
                    status = 200
                };

                
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddUserPersistence(CreateUser createUser)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExists = await accountsContext.Users.FirstOrDefaultAsync(x => x.Email == createUser.Email);
                if (userExists != null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User already exist",
                        status = 400
                    };
                }
                await accountsContext.Users.AddAsync(new User
                {

                    IdUserGroup = createUser.IdUserGroup,
                    Name = createUser.Name,
                    Email = createUser.Email,
                    Password = createUser.Password,
                    DateOfBirth = createUser.DateOfBirth,
                    Genre = createUser.Genre,
                    IsActive = true,
                    IsAdmin = false,
                    IsManager = false,
                    AllowWalletWatch = createUser.AllowWalletWatch

                });
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "User added successfully",
                    status = 200
                };

            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> UpdateUserPersistence(Guid userId, CreateUser createUser)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }


                userExist.IdUserGroup = createUser.IdUserGroup;
                userExist.Name = createUser.Name;
                userExist.Email = createUser.Email;
                userExist.Password = createUser.Password;
                userExist.DateOfBirth = createUser.DateOfBirth;
                userExist.Genre = createUser.Genre;
                userExist.IsActive = createUser.IsActive;
                userExist.IsAdmin = createUser.IsAdmin;
                userExist.IsManager = createUser.IsManager;
                userExist.AllowWalletWatch = createUser.AllowWalletWatch;

                accountsContext.Users.Update(userExist);
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "User added successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteUserPersistence(Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }
                string query = "update public.user " +
                   "set is_active = 'false' " +
                   "where id_user = @IdUser";
                var result2 = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "User removed successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> ActiveUserPersistence(Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }
                string query = "update public.user " +
                   "set is_active = 'active' " +
                   "where id_user = @IdUser";
                var result2 = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "User added successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> GetUsersPersistence()
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                string query = "select * from public.user";
                List<User> listUsers = await accountsContext.Users.FromSqlRaw(query).ToListAsync();
                return new CustomHttpResponse()
                {
                    status = 200,
                    Data = listUsers
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> GetUserByIdPersistence(Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userIdExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser.Equals(userId));
                if (userIdExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "User does not exist",
                        status = 400
                    };
                }
                string query = "select * from public.user where id_user = @IdUSer";
                var user = await accountsContext.Users.FromSqlRaw(query, new NpgsqlParameter("@IdUser", userId)).FirstOrDefaultAsync();
                return new CustomHttpResponse()
                {
                    status = 200,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
    }
}
