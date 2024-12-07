using System.Security.Claims;
using System.Text;
using BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;
using Paseto;
using Paseto.Builder;
using Paseto.Cryptography.Key;

namespace BudgifyAPI.Auth.CA.Entities;

public static class PasetoManager
{
    public static string GeneratePasetoToken(Guid userId, string role)
    {

        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");
        var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();
        Byte[]key = Convert.FromBase64String(config["keys:paseto-key"]);
        
        var token = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local)
            .WithKey(key, Encryption.SymmetricKey)
            .Issuer("https://github.com/RobertoFMBarreto/budgify-api")
            .NotBefore(DateTime.UtcNow)
            .IssuedAt(DateTime.UtcNow)
            .Expiration(DateTime.UtcNow.AddMinutes(15))
            .Subject(userId.ToString())
            .AddClaim(ClaimTypes.Role, role)
            .TokenIdentifier(Guid.NewGuid().ToString())
            .Encode();
       
        return token;
    }
    
    public static string GenerateRefreshPasetoToken(Guid userId)
    {

        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");
        var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();
        Byte[]key = Convert.FromBase64String(config["keys:paseto-key"]);
        
        var token = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local)
            .WithKey(key, Encryption.SymmetricKey)
            .Issuer("https://github.com/RobertoFMBarreto/budgify-api")
            .NotBefore(DateTime.UtcNow)
            .IssuedAt(DateTime.UtcNow)
            .Expiration(DateTime.UtcNow.AddYears(1))
            .Subject(userId.ToString())
            .TokenIdentifier(Guid.NewGuid().ToString())
            .Encode();
       
        return token;
    }

    public static PasetoTokenValidationResult DecodePasetoToken(string token)
    {
        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");
        var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

        Byte[]key = Convert.FromBase64String(config["keys:paseto-key"]);
        var valParams = new PasetoTokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidIssuer = "https://github.com/RobertoFMBarreto/budgify-api"
        };
            
        var result = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local)
            .WithKey(key, Encryption.SymmetricKey).Decode(token,valParams);
        return result;
    }
}