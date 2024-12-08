using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Authservice;
using BudgifyAPI.Gateway.UseCases;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Paseto;
using Paseto.Builder;

namespace BudgifyAPI.Gateway.Entities;

public class PasetoAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public PasetoAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        if (Request.Path.ToString() == "/gateway/accounts/user" && Request.Path == "POST")
        {
            List<Claim> claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        try
        {
            Byte[] key =
                Convert.FromBase64String(Environment.GetEnvironmentVariable(
                    "keys__paseto_key"));

            var valParams = new PasetoTokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidIssuer = "https://github.com/RobertoFMBarreto/budgify-api"
            };
            var result = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local)
                .WithKey(key, Encryption.SymmetricKey).Decode(token, valParams);
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
            var validateRefToken =
                await AuthServiceClient.ValidateRefreshTokenAsync(token,
                    $"{userAgent} / {Context.Connection.RemoteIpAddress}");
            if (!validateRefToken)
                return AuthenticateResult.Fail("Invalid Session");


            List<string> allowedRoles = new List<string>();
            string path = Request.Path.ToString();
            if (path.Contains("/accounts/superadmin"))
            {
                allowedRoles.AddRange(["superadmin"]);
            }
            else if (path.Contains("/accounts/admin"))
            {
                allowedRoles.AddRange(["admin"]);
            }
            else if (path.Contains("/accounts/manager"))
            {
                allowedRoles.AddRange(["admin", "manager"]);
            }
            else if (path.Contains("/accounts/user"))
            {
                allowedRoles.AddRange(["admin", "manager", "user"]);
            }
            else
            {
                allowedRoles.AddRange(["admin", "manager", "user"]);
            }

            string userRole = result.Paseto.Payload["role"].ToString();

            bool isAuthorized = allowedRoles.Contains(userRole);
            if (!isAuthorized)
            {
                return AuthenticateResult.Fail("Not Enough Permissions");
            }

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return AuthenticateResult.Fail("Invalid token");
        }
    }
}