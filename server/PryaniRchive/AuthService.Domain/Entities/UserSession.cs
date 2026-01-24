using System.Security.Cryptography;
using Common.Data;

namespace AuthService.Domain.Entities;

public class UserSession: EntityBase
{
    public Guid UserId { get; init; }

    public required string DeviceId { get; init; }
    
    public required string TokenHash { get; init; }

    public DateTime TokenExpiresAt { get; private set; }
    
    public DateTime? RevokedAt { get; private set; }
    
    public void Revoke(DateTime dateTime) => RevokedAt = dateTime;
    
    private UserSession() {}

    public static UserSession Create(string refreshToker, string deviceId, Guid userId, DateTime expiresAt)
    {
        return new UserSession
        {
            UserId = userId,
            DeviceId = deviceId,
            TokenExpiresAt = expiresAt,
            TokenHash = HashToken(refreshToker)
        };
    }
    
    private static string HashToken(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToHexString(SHA256.HashData(bytes));
    }
}
