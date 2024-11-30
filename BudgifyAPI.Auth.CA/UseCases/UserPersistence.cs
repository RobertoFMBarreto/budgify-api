using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;
using BudgifyAPI.Auth.CA.Entities.Responses;
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
            string refreshToken = PasetoManager.GenerateRefreshPasetoToken(user.IdUser);
            string hash = CustomEncryptor.EncryptString(refreshToken);
            
            user.RefreshToken = hash;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            
            return new CustomHttpResponse()
            {
                data = new LoginResponse()
                {
                    Token = token,
                    RefreshToken = refreshToken
                },
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
            CustomHttpResponse resp = await userEntity.Validate();
            if (resp.status != 200)
            {
                return resp;
            }
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
            
            await context.SaveChangesAsync();
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

    public static async Task<CustomHttpResponse> NewPasswordPersistence(NewPasswordRequest request)
    {
        UserContext context = new UserContext();
        try
        {
            bool isValid = request.validate();

            if (!isValid)
            {
                return new CustomHttpResponse()
                {
                    message = "missing fields",
                    status = 400
                };
            }
            
            User? user = await context.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomHttpResponse()
                {
                    message = "Email or password is incorrect",
                    status = 400
                };
            }
            
            bool isSamePassword = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password);
            if (!isSamePassword)
            {
                return new CustomHttpResponse()
                {
                    message = "Old password does not match",
                    status = 400
                };
            }
            
            string hash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.Password = hash;
            user.RefreshToken = null;
            context.Users.Update(user);
            await context.SaveChangesAsync();
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
    
    public static async Task<CustomHttpResponse> NewSessionTokenPersistence(NewSessionTokenRequest request)
    {
        UserContext context = new UserContext();
        try
        {
            bool isValid = request.validate();

            if (!isValid)
            {
                return new CustomHttpResponse()
                {
                    message = "Missing fields",
                    status = 400
                };
            }
            
            User? user = await context.Users.Where(u => u.Email == CustomEncryptor.EncryptString(request.RefreshToken)).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomHttpResponse()
                {
                    message = "Invalid request",
                    status = 400
                };
            }
            
            var token = PasetoManager.GeneratePasetoToken(user.IdUser);
            return new CustomHttpResponse()
            {
                data = new TokenResponse()
                {
                    Token = token,
                },
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