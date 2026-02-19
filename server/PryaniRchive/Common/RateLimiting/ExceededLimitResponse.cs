namespace Common.RateLimiting;

public record ExceededLimitResponse(double RetryAfterSeconds)
{
    public string Error { get; } = "TooManyRequests";

    public string Message { get; } = "Api limit exceeded.";
}
