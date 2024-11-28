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
            if (String.IsNullOrEmpty(userEntity.Email) || String.IsNullOrEmpty(userEntity.Password))
            {
                return new CustomHttpResponse()
                {
                    message = "Email or password is empty",
                    status = 400
                };
            }
            
            User? user = await context.Users.Where(u=> u.Email == userEntity.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                return new CustomHttpResponse()
                {
                    message = "Email or password is incorrect",
                    status = 400
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(userEntity.Password, user.Password))
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
    public static async Task<CustomHttpResponse> UserRegisterPersistence(UserEntity userEntity)
    {
        UserContext context = new UserContext();
        try
        {
            await userEntity.Validate();
            
            User? user = await context.Users.Where(u=> u.Email == userEntity.Email).FirstOrDefaultAsync();

            if (user != null)
            {
                return new CustomHttpResponse()
                {
                    message = "Email already exists",
                    status = 400
                };
            }
            
            string hash = BCrypt.Net.BCrypt.HashPassword(userEntity.Password);

            await context.Users.AddAsync(new User()
            {
                Email = userEntity.Email,
                Password = hash,
                DateOfBirth = userEntity.DateOfBirth,
                Genre = userEntity.Genre,
                Name = userEntity.Name,
            });
            
            return new CustomHttpResponse()
            {
                message = "success",
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