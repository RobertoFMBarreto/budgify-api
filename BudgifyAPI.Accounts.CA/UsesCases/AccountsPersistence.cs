using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BudgifyAPI.Accounts.CA.UsesCases
{
    public static class AccountsPersistence
    {
        public static async Task<CustomHttpResponse> GetGroupsPersistence(Guid userGroupId)
        {
            AccountsContext accountsContext = new AccountsContext();
            try
            {
                var UserExistGroup = await accountsContext.UserGroups.FirstOrDefaultAsync(x => x.IdUserGroup == userGroupId);
                if (UserExistGroup == null)
                {
                    return new CustomHttpResponse()
                    {
                        message = "caralhoosooooooooos A minha sogra é um boi",
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
    }
}
