using BudgifyAPI.Accounts.CA.Controllers.Requests;
using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Entities.Requests;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
                    var resp = accountsContext.Database.ExecuteSqlRaw(queryUserGroupNull,new NpgsqlParameter("@id_user_group", userGroupId));
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
                        message = "O utilizador não está atribuído a um grupo",
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
    }
}
