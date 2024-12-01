using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Authservice;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Paseto;
using Paseto.Builder;

namespace BudgifyAPI.Gateway.Entities;

public class PasetoAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public PasetoAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        try
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
            if (!result.IsValid)
            {
                return AuthenticateResult.Fail("Invalid Token");
            }
            
            var claims = new[] { new Claim(ClaimTypes.Name, result.Paseto.Payload["sub"].ToString()) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            Context.User = principal;
            var userAgent = Request.Headers["User-Agent"];
            var validateRefToken = await AuthServiceClient.ValidateRefreshTokenAsync(token,$"{userAgent} / {Context.Connection.RemoteIpAddress}");

            Console.WriteLine(validateRefToken);
            if(!validateRefToken)
                return AuthenticateResult.Fail("Invalid Session");

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return AuthenticateResult.Fail("Invalid token");
        }
    }
}