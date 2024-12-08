using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BudgifyAPI.Accounts.CA.UsesCases;

public static class AccountsPersistenceGrpc
{
    public static async Task<User?> GetUserByIdPersistenceGrpc(Guid userId)
    {
        AccountsContext accountsContext = new AccountsContext();
        try
        {
            var userIdExist = await accountsContext.Users.FirstOrDefaultAsync(x => x.IdUser.Equals(userId));
            if (userIdExist == null)
            {
                return null;
            }
            string query = "select * from public.user where id_user = @IdUSer";
            var user = await accountsContext.Users.FromSqlRaw(query, new NpgsqlParameter("@IdUser", userId)).FirstOrDefaultAsync();
            return user;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public static async Task<User?> ValidateUserPersistence(string email, string password)
    {
        AccountsContext accountsContext = new AccountsContext();
        try
        {
            var userExists = await accountsContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userExists == null)
            {
                return null;
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password,userExists.Password);
            if(!isValid)
                return null;
            return userExists;

        }
        catch (Exception ex)
        {
            return null;
        }
    }
}