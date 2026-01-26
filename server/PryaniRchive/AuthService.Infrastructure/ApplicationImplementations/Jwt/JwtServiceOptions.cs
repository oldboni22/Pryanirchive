namespace AuthService.Infrastructure.ApplicationImplementations.Jwt;

public class JwtServiceOptions
{
    public const string SectionName = "Jwt";
    
    public required string Issuer { get; init; }
    
    public required string Audience { get; init; }
    
    public required string SignatureKey { get; init; }
    
    public int AccessTokenLifetimeMinutes { get; init; }
    
    public int RefreshTokenLifetimeDays { get; init; }
}
