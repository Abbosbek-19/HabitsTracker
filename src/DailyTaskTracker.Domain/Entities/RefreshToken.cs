namespace DailyTaskTracker.Domain.Entities;

/// <summary>
/// Persisted JWT Refresh Token for secure session management.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    private RefreshToken() { }

    public RefreshToken(string token, DateTime expiresAtUtc, Guid userId)
    {
        Id = Guid.NewGuid();
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
        UserId = userId;
        IsRevoked = false;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Revoke() => IsRevoked = true;
    public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAtUtc;
}
