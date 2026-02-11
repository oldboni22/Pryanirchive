using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Common.Logging;

/// <summary>
/// Middleware that adds correlation IDs to requests for distributed tracing
/// </summary>
public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Try to get correlation ID from request headers
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault();
        
        // If not present, generate a new one
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        // Store in HttpContext for access throughout the request pipeline
        context.Items["CorrelationId"] = correlationId;

        // Add to response headers so clients can track requests
        context.Response.Headers.TryAdd(CorrelationIdHeaderName, correlationId);

        // Push correlation ID to Serilog's LogContext
        // This automatically adds it to all log entries in this request scope
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
