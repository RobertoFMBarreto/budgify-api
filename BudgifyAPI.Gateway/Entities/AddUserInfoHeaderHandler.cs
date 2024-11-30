using System.Security.Claims;
using System.Text;

namespace BudgifyAPI.Gateway.Entities;

public class AddUserInfoHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddUserInfoHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;
        
        if (user.Identity.IsAuthenticated)
        {
            var userId = user.FindFirst(ClaimTypes.Name)?.Value;
            if (userId != null)
            {
                string enc = CustomEncryptor.EncryptString(user.Identity.Name);
                request.Headers.Add("X-User-Id", Convert.ToBase64String(Encoding.UTF8.GetBytes(enc)));
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}