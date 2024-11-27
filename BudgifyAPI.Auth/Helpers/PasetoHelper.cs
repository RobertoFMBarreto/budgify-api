using BudgifyAPI.Auth.Helpers.Interfaces;
using Paseto.Builder;

namespace BudgifyAPI.Auth.Helpers;
using Paseto;
public class PasetoHelper:IPasetoHelper
{
     
     public string GeneratePasetoToken(Guid userId)
     {
          var pasetoKey = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local).GenerateSymmetricKey(); 
          var token = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local)
               .WithKey(pasetoKey)
               //.AddClaim("uid", "this is a secret message")
               //.Issuer("https://github.com/daviddesmet/paseto-dotnet")
               .Subject(userId.ToString())
               //.Audience("https://paseto.io")
               .NotBefore(DateTime.UtcNow.AddMinutes(5))
               .IssuedAt(DateTime.UtcNow)
               .Expiration(DateTime.UtcNow.AddMinutes(15))
               .TokenIdentifier("123456ABCD")
               //.AddFooter("arbitrary-string-that-isn't-json")
               .Encode();
          return token;
     }
     
    
    
}