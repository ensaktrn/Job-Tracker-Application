using System.Security.Claims;

namespace JobTracker.Api.Auth;

public interface ITokenService
{
    string CreateAccessToken(string userId, string email, IEnumerable<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken);
}