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

        Byte[] key =
            Convert.FromBase64String( Environment.GetEnvironmentVariable(
                "keys__paseto_key"));
        
        var token = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local)
            .WithKey(key, Encryption.SymmetricKey)
            .Issuer("https://github.com/RobertoFMBarreto/budgify-api")
            .NotBefore(DateTime.UtcNow)
            .IssuedAt(DateTime.UtcNow)
            .Expiration(DateTime.UtcNow.AddMinutes(15))
            .Subject(userId.ToString())
            .AddClaim("role", role)
            .TokenIdentifier(Guid.NewGuid().ToString())
            .Encode();
       
        return token;
    }
    
    public static string GenerateRefreshPasetoToken(Guid userId)
    {

        Byte[] key =
            Convert.FromBase64String( Environment.GetEnvironmentVariable(
                "keys__paseto_key"));
        
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
        Byte[] key =
            Convert.FromBase64String( Environment.GetEnvironmentVariable(
                "keys__paseto_key"));
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