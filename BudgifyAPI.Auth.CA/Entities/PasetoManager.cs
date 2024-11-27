using Paseto;
using Paseto.Builder;

namespace BudgifyAPI.Auth.CA.Entities;

public static class PasetoManager
{
    public static string GeneratePasetoToken(Guid userId)
    {
        var pasetoKey = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local).GenerateSymmetricKey(); 
        var token = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local)
            .WithKey(pasetoKey)
            .AddClaim("uid", userId)
            .Issuer("https://github.com/RobertoFMBarreto/budgify-api")
            //.Subject(userId.ToString())
            //.Audience("https://paseto.io")
            .NotBefore(DateTime.UtcNow.AddMinutes(5))
            .IssuedAt(DateTime.UtcNow)
            .Expiration(DateTime.UtcNow.AddMinutes(15))
            .TokenIdentifier("123456ABCD")
            //.AddFooter("arbitrary-string-that-isn't-json")
            .Encode();
        return token;
    }

    public static string DecodePasetoToken(string token)
    {
        new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local).Decode(token);
    }
}