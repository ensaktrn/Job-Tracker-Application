namespace JobTracker.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string UserId { get; private set; } = string.Empty;

    public string Token { get; private set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt is not null;
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;

    private RefreshToken() { }

    public RefreshToken(string userId, string token, DateTimeOffset expiresAt)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        Token = token ?? throw new ArgumentNullException(nameof(token));
        ExpiresAt = expiresAt;
    }

    public void Revoke() => RevokedAt = DateTimeOffset.UtcNow;
}