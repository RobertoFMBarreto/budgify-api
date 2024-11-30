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
    public static string GeneratePasetoToken(Guid userId)
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
            .TokenIdentifier("123456ABCD")
            .Encode();
       
        return token;
    }
}