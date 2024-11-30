using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
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

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

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
                return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
            }
            var claims = new[] { new Claim(ClaimTypes.Name, result.Paseto.Payload["sub"].ToString()) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            Context.User = principal;
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }
    }
}