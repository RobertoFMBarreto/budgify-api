using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Auth.CA.UseCases;

public static class UserPersistence
{
    public static async Task<CustomHttpResponse> UserLoginPersistence(UserEntity userEntity)
    {
        UserContext context = new UserContext();
        try
        {
            userEntity.Validate();
            
            User? user = await context.Users.Where(u=> u.Email == userEntity.Email && u.Password == userEntity.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return new CustomHttpResponse()
                {
                    message = "Email or password is incorrect",
                    status = 400
                };
            }

            string token = PasetoManager.GeneratePasetoToken(user.IdUser);
            
            return new CustomHttpResponse()
            {
                message = token,
                status = 200
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new CustomHttpResponse()
            {
                message = e.Message,
                status = 500
            };
        }
    }
}