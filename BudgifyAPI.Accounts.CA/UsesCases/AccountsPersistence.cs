using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Authservice;
using BudgifyAPI.Auth.CA.Entities.Requests;

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
                        Status = 400,
                        Message = "User already has group",
                    };
                }

                UserGroup userGroup = new UserGroup()
                {
                    IdUserGroup = Guid.NewGuid(),
                    Name = name.Name,
                };
                await accountsContext.UserGroups.AddAsync(userGroup);
                
                user.IdUserGroup = userGroup.IdUserGroup;
                user.IsAdmin = true;
                accountsContext.Users.Update(user);
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "User group added successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                        Message = "Group doesn't exist",
                        Status = 400
                    };
                }
                
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                userGroupExist.Name = name.Name;
                accountsContext.UserGroups.Update(userGroupExist);
                await accountsContext.SaveChangesAsync();

                return new CustomHttpResponse()
                {
                    Message = "User group updated successfully",
                    Status = 200
                };
                
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                
                string queryUserGroupNull = "update public.user set id_user_group = null where id_user_group = @IdUserGroup";
                accountsContext.Database.ExecuteSqlRaw(queryUserGroupNull, new NpgsqlParameter("@IdUserGroup", userGroupId));
                accountsContext.UserGroups.Remove(userGroupExist);
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "User group removed successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                return new CustomHttpResponse()
                {
                    Status = 200,
                    Data = userGroupExist,

                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Status = 500,
                    Message = ex.Message
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
                        Status = 200,
                        Message = "User doesn't have group",
                    };
                }
                var userGroups = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == user.IdUserGroup);
                
                return new CustomHttpResponse()
                {
                    Status = 200,
                    Data = userGroups,

                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Status = 500,
                    Message = ex.Message
                };
            }
        }
        public static async Task<CustomHttpResponse> AddUserToUserGroupPersistence(Guid addUserId, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (existUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User not exist",
                        Status = 400
                    };
                }
                
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId &&
                    (x.IsAdmin == true || x.IsManager == true));
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == addUserId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User does not exist",
                        Status = 400
                    };
                }
                if (userExist.IdUserGroup != null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User is already in a user group",
                        Status = 400
                    };
                }
                
                string query = "update public.user " +
                    $"set id_user_group = @IdUserGroup " +
                    $"where id_user = @IdUser";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUserGroup", existUser.IdUserGroup), new NpgsqlParameter("@IdUser", addUserId));
                return new CustomHttpResponse()
                {
                    Message = "User added to a user group successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Status = 500,
                    Message = ex.Message
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteUserFromUserGroupPersistence(Guid removeUserId, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (existUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User not exist",
                        Status = 400
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId &&
                    (x.IsAdmin == true || x.IsManager == true));
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == removeUserId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User does not exist",
                        Status = 400
                    };
                }
                if (userExist.IdUserGroup != existUser.IdUserGroup && userExist.IdUserGroup != null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User is in another group",
                        Status = 400
                    };
                }
                string query = "update public.user " +
                    "set id_user_group = null " +
                    "where id_user = @IdUser ";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", removeUserId));
                return new CustomHttpResponse()
                {
                    Message = "User removed successfully",
                    Status = 200
                };

            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> AddManagerToUserGroupPersistence(Guid managerId, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (existUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User not exist",
                        Status = 400
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                Console.WriteLine(managerId);
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == managerId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User does not exist",
                        Status = 400
                    };
                }
                if (userExist.IdUserGroup != null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User is already in a user group",
                        Status = 400
                    };
                }
                string query = "update public.user " +
                  $"set id_user_group = @IdUserGroup " +
                  $", is_Manager = true " +
                  $"where id_user = @IdUser";

                await accountsContext.Database.ExecuteSqlRawAsync(query,
                    new NpgsqlParameter("@IdUserGroup", existUser.IdUserGroup),
                    new NpgsqlParameter("@IdUser", managerId));

                return new CustomHttpResponse()
                {
                    Message = "Manager added successfully",
                    Status = 200
                }; 
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }
        public static async Task<CustomHttpResponse> DeleteManagerToUserGroupPersistence(Guid removeManagerId, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (existUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User not exist",
                        Status = 400
                    };
                }
                
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == removeManagerId);
                if (userExist == null)
                    return new CustomHttpResponse()
                    {
                        Message = "Manager does not exist",
                        Status = 400
                    };
                
                var existGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == existUser.IdUserGroup);
                if (existGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User does not exist",
                        Status = 400
                    };
                }
                var isFromUser = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId && x.IsAdmin == true);
                if (isFromUser == null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "Group doesn't exist",
                    };
                }
                
                if (userExist.IdUserGroup != existGroup.IdUserGroup && userExist.IdUserGroup != null)
                {
                    return new CustomHttpResponse()
                    {
                        Status = 400,
                        Message = "User is in another group",
                    };
                }
                
                
                string query = "update public.user " +
                "set is_manager = false ," +
                "id_user_group = null " +
                "where id_user = @IdUser ";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", removeManagerId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "Manager removed successfully",
                    Status = 200
                };

                
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                        Message = "User already exist",
                        Status = 400
                    };
                }
                string hash = BCrypt.Net.BCrypt.HashPassword(createUser.Password);
                await accountsContext.Users.AddAsync(new User
                {
                    IdUserGroup = createUser.IdUserGroup,
                    Name = createUser.Name,
                    Email = createUser.Email,
                    Password = hash,
                    DateOfBirth = createUser.DateOfBirth,
                    Genre = createUser.Genre,
                    IsActive = true,
                    IsAdmin = false,
                    IsManager = false,
                    IsSuperAdmin = false,
                    AllowWalletWatch = createUser.AllowWalletWatch

                });
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "User added successfully",
                    Status = 200
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }
        
        
        public static async Task<CustomHttpResponse> NewPasswordPersistence(NewPasswordRequest request)
        {
            AccountsContext context = new AccountsContext();
            try
            {
                bool isValid = request.Validate();

                if (!isValid)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "missing fields",
                        Status = 400
                    };
                }
            
                User? user = await context.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Email or password is incorrect",
                        Status = 400
                    };
                }
            
                bool isSamePassword = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password);
                if (!isSamePassword)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "Old password does not match",
                        Status = 400
                    };
                }
            
                string hash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.Password = hash;
                await AuthServiceClient.LogoutUser(user.IdUser.ToString());
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "success",
                    Status = 200
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new CustomHttpResponse()
                {
                    Message = e.Message,
                    Status = 500
                };
            }
        }
        
        public static async Task<CustomHttpResponse> UpdateUserPersistence(Guid userId, UpdateUser updateUser)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        Message = "User does not exist",
                        Status = 400
                    };
                }


                userExist.IdUserGroup = updateUser.IdUserGroup;
                userExist.Name = updateUser.Name;
                userExist.Email = updateUser.Email;
                userExist.DateOfBirth = updateUser.DateOfBirth;
                userExist.Genre = updateUser.Genre;
                userExist.IsActive = true;
                userExist.IsAdmin = false;
                userExist.IsManager = false;
                userExist.IsSuperAdmin = false;
                userExist.AllowWalletWatch = updateUser.AllowWalletWatch;

                accountsContext.Users.Update(userExist);
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "User updated successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                        Message = "User does not exist",
                        Status = 400
                    };
                }
                string query = "update public.user " +
                   "set is_active = 'false' " +
                   "where id_user = @IdUser";
                var result2 = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "User removed successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                        Message = "User does not exist",
                        Status = 400
                    };
                }
                string query = "update public.user " +
                   "set is_active = 'true' " +
                   "where id_user = @IdUser";
                var result2 = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@IdUser", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    Message = "User added successfully",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                    Status = 200,
                    Data = listUsers
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
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
                        Message = "User does not exist",
                        Status = 400
                    };
                }
                string query = "select * from public.user where id_user = @IdUSer";
                var user = await accountsContext.Users.FromSqlRaw(query, new NpgsqlParameter("@IdUser", userId)).FirstOrDefaultAsync();
                return new CustomHttpResponse()
                {
                    Status = 200,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse()
                {
                    Message = ex.Message,
                    Status = 500
                };
            }
        }
        
        
    }
}
