using System.Threading.RateLimiting;
using Common.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.RateLimiting;

file sealed class RateLimitLogs
{
}

public static class RateLimitExtensions
{
    private const string ForwardedHeaderKey = "X-Forwarded-For";

    private const string DefaultPartitionKey = "No-forwarded-for";

    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureRateLimiting(IConfiguration configuration)
        {
            return services
                .Configure<RateLimitingOptions>(configuration.GetSection(RateLimitingOptions.ConfigurationSection))
                .AddRateLimiter(options =>
                {
                    options.OnRejected = CreateOnRejected();

                    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    {
                        var endpoint = context.GetEndpoint();

                        var hasSpecificPolicy = endpoint?.Metadata.GetMetadata<EnableRateLimitingAttribute>() != null;

                        return hasSpecificPolicy
                            ? RateLimitPartition.GetNoLimiter("Excluded global")
                            : CreateGlobalRateLimiter(context);
                    });
                });
        }
    }

    extension(HttpContext context)
    {
        private string ExtractPartitionKey()
        {
            var headerContent = context.Request.Headers[ForwardedHeaderKey].ToString();

            if (!string.IsNullOrWhiteSpace(headerContent))
            {
                var ip = headerContent.Split(',')[0].Trim();
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    return ip;
                }
            }

            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            return !string.IsNullOrEmpty(remoteIp)
                ? remoteIp
                : DefaultPartitionKey;
        }

        private void LogExceededLimit(string partitionKey)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<RateLimitLogs>>();

            logger.LogRateLimitExceeded(partitionKey);
        }
    }

    private static RateLimitPartition<string> CreateGlobalRateLimiter(HttpContext context)
    {
        var rateLimitingOptions =
            context.RequestServices.GetRequiredService<IOptionsSnapshot<RateLimitingOptions>>().Value;

        return
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.ExtractPartitionKey(),
                key => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = rateLimitingOptions.Limit,
                    QueueLimit = rateLimitingOptions.QueueLimit,
                    Window = TimeSpan.FromSeconds(rateLimitingOptions.RefreshSeconds)
                });
    }

    private static Func<OnRejectedContext, CancellationToken, ValueTask> CreateOnRejected()
    {
        return async (context, cancellationToken) =>
        {
            var httpContext = context.HttpContext;

            var rateLimitingOptions = httpContext.RequestServices
                .GetRequiredService<IOptionsSnapshot<RateLimitingOptions>>().Value;

            var partitionKey = httpContext.ExtractPartitionKey();
            httpContext.LogExceededLimit(partitionKey);

            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            TimeSpan? retryAfter = null;
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue))
            {
                retryAfter = retryAfterValue;
                httpContext.Response.Headers.RetryAfter = ((int)retryAfter.Value.TotalSeconds).ToString();
            }

            var response = new ExceededLimitResponse(
                Math.Round(retryAfter?.TotalSeconds ?? rateLimitingOptions.RefreshSeconds));

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        };
    }
}