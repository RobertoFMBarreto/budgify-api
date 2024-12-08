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
        
        
        
        if (_httpContextAccessor.HttpContext.Request.Path.ToString() == "/gateway/accounts/user" && _httpContextAccessor.HttpContext.Request.Method == "POST")
        {
            return base.SendAsync(request, cancellationToken);
        }
        
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