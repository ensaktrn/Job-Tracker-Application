using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;
using JobTracker.Api.Auth;
using JobTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly JwtOptions _jwt;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        AppDbContext db,
        ITokenService tokenService,
        IOptions<JwtOptions> jwt)
    {
        _userManager = userManager;
        _db = db;
        _tokenService = tokenService;
        _jwt = jwt.Value;
    }

    public sealed record RegisterRequest(string Email, string Password);
    public sealed record LoginRequest(string Email, string Password);
    public sealed record RefreshRequest(string AccessToken, string RefreshToken);

    public sealed record AuthResponse(string AccessToken, string RefreshToken);

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = new ApplicationUser { UserName = email, Email = email };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        // default role (opsiyonel)
        // await _userManager.AddToRoleAsync(user, "User");

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user is null)
            return Unauthorized();

        var ok = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!ok)
            return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _tokenService.CreateAccessToken(user.Id, user.Email!, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshExpires = DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenDays);

        _db.RefreshTokens.Add(new RefreshToken(user.Id, refreshToken, refreshExpires));
        await _db.SaveChangesAsync(ct);

        return Ok(new AuthResponse(accessToken, refreshToken));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal is null) return Unauthorized();

        var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var stored = await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, ct);

        if (stored is null || stored.UserId != userId || stored.IsExpired || stored.IsRevoked)
            return Unauthorized();

        stored.Revoke(); // rotate: eski refresh token iptal

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);

        var newAccess = _tokenService.CreateAccessToken(user.Id, user.Email!, roles);
        var newRefresh = _tokenService.GenerateRefreshToken();

        _db.RefreshTokens.Add(new RefreshToken(user.Id, newRefresh, DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenDays)));
        await _db.SaveChangesAsync(ct);

        return Ok(new AuthResponse(newAccess, newRefresh));
    }
}
