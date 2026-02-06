using System.Security.Claims;
using JobTracker.Application.Common;

namespace JobTracker.Api.Auth;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http) => _http = http;

    public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public string UserId =>
        _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("User is not authenticated.");
    
    public string UserEmail =>
        _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)
        ?? throw new InvalidOperationException("User email claim not found.");
}