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
        public static async Task<CustomHttpResponse> AddUserGroupPersistence(RequestName name)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                await accountsContext.UserGroups.AddAsync(new UserGroup() { Name = name.name });
                if (accountsContext != null)
                {
                    await accountsContext.SaveChangesAsync();
                    return new CustomHttpResponse()
                    {
                        message = "Grupo criado com sucesso",
                        status = 200
                    };

                }
                return new CustomHttpResponse()
                {
                    message = "Occurreu um erro",
                    status = 400
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
        public static async Task<CustomHttpResponse> UpdateUserGroupPersistence(Guid userGroupId, RequestName name)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userGroupExist = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (userGroupExist != null)
                {
                    accountsContext.UserGroups.Update(new UserGroup() { Name = name.name });
                    await accountsContext.SaveChangesAsync();

                    return new CustomHttpResponse()
                    {
                        message = "Grupo atualizado com sucesso",
                        status = 200
                    };
                }
                return new CustomHttpResponse()
                {
                    message = "Ocurreu um erro",
                    status = 400
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
        public static async Task<CustomHttpResponse> DeleteUserGroupPersistence(Guid userGroupId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userGroupExist = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (userGroupExist != null)
                {
                    string queryUserGroupNull = "update public.user set id_user_group = null where id_user_group = @userGroup";
                    var resp = accountsContext.Database.ExecuteSqlRaw(queryUserGroupNull, new NpgsqlParameter("@id_user_group", userGroupId));
                    accountsContext.UserGroups.Remove(userGroupExist);
                    await accountsContext.SaveChangesAsync();
                    return new CustomHttpResponse()
                    {
                        message = "Grupo removido com sucesso",
                        status = 200
                    };
                }
                return new CustomHttpResponse()
                {
                    message = "Occureu um erro",
                    status = 400
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
        public static async Task<CustomHttpResponse> GetGroupsByIdPersistence(Guid userGroupId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var UserExistGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (UserExistGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Occurreu um erro",
                        status = 400,
                    };
                }
                string query = $"select * from public.user_group where id_user_group =@userGroupId";
                var listaUserGroups = await accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@userGroupId", userGroupId));
                return new CustomHttpResponse()
                {
                    status = 200,
                    Data = listaUserGroups,

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
        public static async Task<CustomHttpResponse> AddUserToUserGroupPersistence(CreateUser user, Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var existGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == user.IdUserGroup);
                if (existGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "O utilizador não existe",
                        status = 400
                    };
                }
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "O grupo não existe",
                        status = 400
                    };
                }
                if (userExist.IdUserGroup != null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "O utilizador já está atribuído a um grupo",
                        status = 400
                    };
                }
                string query = "update public.user " +
                    $"set id_user_group = @id_user_group " +
                    $"where id_user = @id_user";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@id_user_group", user.IdUserGroup), new NpgsqlParameter("@id_user", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "O utilizador associado a um grupo com sucesso",
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
        public static async Task<CustomHttpResponse> DeleteUserFromUserGroupPersistence(Guid userId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                if (userExist == null)
                    return new CustomHttpResponse()
                    {
                        message = "O utilizador não existe",
                        status = 400
                    };
                string query = "update public.user " +
                    "set id_user_group = null " +
                    "where id_user = @id_user ";
                var result = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@id_user", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Utilizador removido do grupo com sucesso",
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
        public static async Task<CustomHttpResponse> AddManagerToUserGroupPersistence(User user)
        {
            //primeiro adicionar ao grupo e depois tornar manager - FEITO - REVER
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var userExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser == user.IdUser);
                if (userExist == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "Utilizador não existe",
                        status = 400
                    };
                }
                string query = "update public.user " +
                  $"set id_user_group = @id_user_group " +
                  $"set is_Manager = true, " +
                  $"where id_user = @id_user";

                await accountsContext.Database.ExecuteSqlRawAsync(query,
                    new NpgsqlParameter("@id_user_group", user.IdUserGroup),
                    new NpgsqlParameter("@id_user", user.IdUser));

                accountsContext.SaveChangesAsync();

                return new CustomHttpResponse()
                {
                    message = "Gestor adicionado ao grupo com sucesso",
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
                        message = "Utilizador já existe",
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
                    IsActive = createUser.IsActive,
                    IsAdmin = createUser.IsAdmin,
                    IsManager = createUser.IsManager,
                    AllowWalletWatch = createUser.AllowWalletWatch

                });
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Utilizador adicionado com sucesso",
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
                        message = "Utilizador não existe",
                        status = 400
                    };
                }

                accountsContext.Users.Update(new User()
                {
                    IdUserGroup = createUser.IdUserGroup,
                    Name = createUser.Name,
                    Email = createUser.Email,
                    Password = createUser.Password,
                    DateOfBirth = createUser.DateOfBirth,
                    Genre = createUser.Genre,
                    IsActive = createUser.IsActive,
                    IsAdmin = createUser.IsAdmin,
                    IsManager = createUser.IsManager,
                    AllowWalletWatch = createUser.AllowWalletWatch
                });
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Utilizador atualizado com sucesso",
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
                        message = "Utilizador não existe",
                        status = 400
                    };
                }
                string query = "update public.user " +
                   "set is_active = 'false' " +
                   "where id_user = @id_user";
                var result2 = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@id_user", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Utilizador removido com sucesso",
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
                        message = "Utilizador não existe",
                        status = 400
                    };
                }
                string query = "update public.user " +
                   "set is_active = 'active' " +
                   "where id_user = @id_user";
                var result2 = accountsContext.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@id_user", userId));
                await accountsContext.SaveChangesAsync();
                return new CustomHttpResponse()
                {
                    message = "Utilizador atualizado com sucesso",
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
                        message = "Não existem utilizadores",
                        status = 400
                    };
                }
                string query = "select * from public.user where id_user = @id_user";
                var user = await accountsContext.Users.FromSqlRaw(query, new NpgsqlParameter("@id_user", userId)).FirstOrDefaultAsync();
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
