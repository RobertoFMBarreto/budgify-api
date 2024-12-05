using Authservice;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;
using BudgifyAPI.Auth.CA.Entities.Responses;
using BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;


namespace BudgifyAPI.Auth.CA.UseCases;

public static class UserPersistence
{
    public static async Task<CustomHttpResponse> UserLoginPersistence(UserEntity userEntity, string userAgent)
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
            Console.WriteLine(userAgent);
            UserRefreshToken? refToken = await context.UserRefreshTokens.Where(userRefreshToken => userRefreshToken.Device == userAgent)
                .FirstOrDefaultAsync();
            if (refToken == null)
            {
                
                string hash = CustomEncryptor.EncryptString(refreshToken);
                await context.UserRefreshTokens.AddAsync(new UserRefreshToken()
                {
                    Token = hash,
                    Device = userAgent,
                    IdUser = user.IdUser,
                });
            }
            else
            {
                refreshToken = CustomEncryptor.DecryptString(refToken.Token);
            }
            
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

    public static async Task<CustomHttpResponse> UserLogoutPersistence(string uid, string device)
    { 
        UserContext context = new UserContext();
        try
        {
            User? user = await context.Users.Where(u => u.IdUser == Guid.Parse(uid)).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomHttpResponse()
                {
                    message = "Email or password is incorrect",
                    status = 400
                };
            }
            
            UserRefreshToken? refToken = await context.UserRefreshTokens.Where(ur=>ur.Device==device).FirstOrDefaultAsync();
            context.UserRefreshTokens.Remove(refToken);
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
            throw;
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
            context.UserRefreshTokens.RemoveRange(context.UserRefreshTokens.Where(ur=>ur.IdUser==user.IdUser));
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
            
            UserRefreshToken? userRefresh = await context.UserRefreshTokens.Where(u => u.Token == CustomEncryptor.EncryptString(request.RefreshToken)).FirstOrDefaultAsync();
            if (userRefresh == null)
            {
                return new CustomHttpResponse()
                {
                    message = "Invalid request",
                    status = 400
                };
            }
            
            var token = PasetoManager.GeneratePasetoToken(userRefresh.IdUser);
            
            userRefresh.LastUsage = DateTime.Now;
            context.UserRefreshTokens.Update(userRefresh);
            await context.SaveChangesAsync();
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
    
    public static async Task<bool> ValidateSessionPersistence(ValidateTokenRequest request)
    {
        UserContext context = new UserContext();
        try
        {
            var token = PasetoManager.DecodePasetoToken(CustomEncryptor.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.Token))));
            if (!token.IsValid)
                return false;
            var uid = token.Paseto.Payload["sub"].ToString();
            UserRefreshToken? user = await context.UserRefreshTokens.Where(u=> u.IdUser == Guid.Parse(uid) && u.Device == request.Agent).FirstOrDefaultAsync();
            Console.WriteLine(user==null);
            if(user == null)
                return false;

            var result = PasetoManager.DecodePasetoToken(CustomEncryptor.DecryptString(user.Token));
 
            return result.IsValid;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}