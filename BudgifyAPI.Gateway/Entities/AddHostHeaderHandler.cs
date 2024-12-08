namespace BudgifyAPI.Gateway.Entities;

public class AddHostHeaderHandler: DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddHostHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-User-Ip", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());

        return base.SendAsync(request, cancellationToken);
    }
}