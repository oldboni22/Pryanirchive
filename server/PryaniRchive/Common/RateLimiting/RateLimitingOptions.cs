namespace Common.RateLimiting;

public class RateLimitingOptions
{
    public const string ConfigurationSection = "RateLimiting";
    
    public int RefreshSeconds { get; init; } = 60;
    
    public int Limit { get; init; } = 30;

    public int QueueLimit { get; init; } = 2;
}
