using System.Text;
using Authservice;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.Entities.Requests;
using BudgifyAPI.Auth.CA.Entities.Responses;
using BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Validateuserservice;


namespace BudgifyAPI.Auth.CA.UseCases;

public static class UserPersistence
{
    public static async Task<CustomHttpResponse> UserLoginPersistence(UserEntity userEntity, string userAgent)
    {
        AuthenticationContext context = new AuthenticationContext();
        try
        {
            if (String.IsNullOrEmpty(userEntity.Email) || String.IsNullOrEmpty(userEntity.Password))
            {
                return new CustomHttpResponse()
                {
                    Message = "Email or password is empty",
                    Status = 400
                };
            }

            ValidateUserResponse validateUserResponse = await
                ValidateUserClient.ValidateUserAsync(userEntity.Email, userEntity.Password);
            if (!validateUserResponse.IsValid)
            {
                return new CustomHttpResponse()
                {
                    Status = 400,
                    Message = "Email or password is wrong",
                };
            }
            string role = "user";
            if (validateUserResponse.IsAdmin)
            {
                role = "admin";
            }else if (validateUserResponse.IsManager)
            {
                role = "manager";
            }else if (validateUserResponse.IsSuperadmin)
            {
                role = "superadmin";
            }
            string token = PasetoManager.GeneratePasetoToken(Guid.Parse(validateUserResponse.Uid), role);
            string refreshToken = PasetoManager.GenerateRefreshPasetoToken(Guid.Parse(validateUserResponse.Uid));
            
            UserRefreshToken? refToken = await context.UserRefreshTokens.Where(userRefreshToken => userRefreshToken.Device == userAgent && userRefreshToken.IdUser == Guid.Parse(validateUserResponse.Uid))
                .FirstOrDefaultAsync();
            if (refToken == null)
            {
                
                string hash = CustomEncryptor.EncryptString(refreshToken);
                await context.UserRefreshTokens.AddAsync(new UserRefreshToken()
                {
                    Token = hash,
                    Device = userAgent,
                    IdUser = Guid.Parse(validateUserResponse.Uid),
                });
            }
            else
            {
                refreshToken = CustomEncryptor.DecryptString(refToken.Token);
                refToken.LastUsage = DateTime.Now;
                context.UserRefreshTokens.Update(refToken);
                await context.SaveChangesAsync();
            }
            
            await context.SaveChangesAsync();
            
            return new CustomHttpResponse()
            {
                Data = new LoginResponse()
                {
                    Token = token,
                    RefreshToken = refreshToken
                },
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

    public static async Task<CustomHttpResponse> UserLogoutPersistence(string uid, string device)
    { 
        AuthenticationContext context = new AuthenticationContext();
        try
        {
            
            
            UserRefreshToken? refToken = await context.UserRefreshTokens.Where(ur=>ur.Device==device).FirstOrDefaultAsync();
            if(refToken != null)
                context.UserRefreshTokens.Remove(refToken);
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
            throw;
        }
        
    }

    
    public static async Task<CustomHttpResponse> NewSessionTokenPersistence(NewSessionTokenRequest request)
    {
        AuthenticationContext context = new AuthenticationContext();
        try
        {
            bool isValid = request.Validate();

            if (!isValid)
            {
                return new CustomHttpResponse()
                {
                    Message = "Missing fields",
                    Status = 400
                };
            }
            
            UserRefreshToken? userRefresh = await context.UserRefreshTokens.Where(u => u.Token == CustomEncryptor.EncryptString(request.RefreshToken)).FirstOrDefaultAsync();
            if (userRefresh == null)
            {
                return new CustomHttpResponse()
                {
                    Message = "Invalid request",
                    Status = 400
                };
            }
            
            ValidateUserResponse validateUserResponse = await
                ValidateUserClient.GetUserRoleAsync(userRefresh.IdUser.ToString());
            if (!validateUserResponse.IsValid)
            {
                return new CustomHttpResponse()
                {
                    Status = 400,
                    Message = "Email or password is wrong",
                };
            }
            string role = "user";
            if (validateUserResponse.IsAdmin)
            {
                role = "admin";
            }else if (validateUserResponse.IsManager)
            {
                role = "manager";
            }else if (validateUserResponse.IsSuperadmin)
            {
                role = "superadmin";
            }
            
            var token = PasetoManager.GeneratePasetoToken(userRefresh.IdUser, role);
            
            userRefresh.LastUsage = DateTime.Now;
            context.UserRefreshTokens.Update(userRefresh);
            await context.SaveChangesAsync();
            return new CustomHttpResponse()
            {
                Data = new TokenResponse()
                {
                    Token = token,
                },
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
    
    public static async Task<bool> ValidateSessionPersistence(ValidateTokenRequest request)
    {
        AuthenticationContext context = new AuthenticationContext();
        try
        {
            var token = PasetoManager.DecodePasetoToken(CustomEncryptor.DecryptString( Encoding.UTF8.GetString( Convert.FromBase64String(request.Token)) ));
            if (!token.IsValid)
                return false;
            var uid = token.Paseto.Payload["sub"].ToString();
            UserRefreshToken? user = await context.UserRefreshTokens.Where(u=> u.IdUser == Guid.Parse(uid) && u.Device == request.Agent).FirstOrDefaultAsync();

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