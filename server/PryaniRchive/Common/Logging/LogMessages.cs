using Microsoft.Extensions.Logging;

namespace Common.Logging;

/// <summary>
/// High-performance logging messages using LoggerMessage source generation.
/// These methods have zero allocation and minimal overhead.
/// </summary>
public static partial class LogMessages
{
    // HTTP Request Logging
    
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} started")]
    public static partial void LogHttpRequestStarted(
        this ILogger logger,
        string method,
        string path);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} completed with status {StatusCode} in {ElapsedMs} ms")]
    public static partial void LogHttpRequestCompleted(
        this ILogger logger,
        string method,
        string path,
        int statusCode,
        long elapsedMs);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Error,
        Message = "HTTP {Method} {Path} failed with status {StatusCode}")]
    public static partial void LogHttpRequestFailed(
        this ILogger logger,
        Exception exception,
        string method,
        string path,
        int statusCode);

    // Database Logging
    
    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Debug,
        Message = "Executing database query: {QueryName} with parameters {Parameters}")]
    public static partial void LogDatabaseQueryStarted(
        this ILogger logger,
        string queryName,
        string parameters);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Information,
        Message = "Database query {QueryName} completed in {ElapsedMs} ms")]
    public static partial void LogDatabaseQueryCompleted(
        this ILogger logger,
        string queryName,
        long elapsedMs);

    [LoggerMessage(
        EventId = 2003,
        Level = LogLevel.Warning,
        Message = "Slow database query detected: {QueryName} took {ElapsedMs} ms")]
    public static partial void LogSlowDatabaseQuery(
        this ILogger logger,
        string queryName,
        long elapsedMs);

    // gRPC Logging
    
    [LoggerMessage(
        EventId = 3001,
        Level = LogLevel.Information,
        Message = "gRPC call to {Service}.{Method} started")]
    public static partial void LogGRpcCallStarted(
        this ILogger logger,
        string service,
        string method);

    [LoggerMessage(
        EventId = 3002,
        Level = LogLevel.Information,
        Message = "gRPC call to {Service}.{Method} completed in {ElapsedMs} ms")]
    public static partial void LogGRpcCallCompleted(
        this ILogger logger,
        string service,
        string method,
        long elapsedMs);

    [LoggerMessage(
        EventId = 3003,
        Level = LogLevel.Error,
        Message = "gRPC call to {Service}.{Method} failed")]
    public static partial void LogGRpcCallFailed(
        this ILogger logger,
        Exception exception,
        string service,
        string method);

    // Authentication/Authorization Logging
    
    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = "User {UserId} authenticated successfully")]
    public static partial void LogUserAuthenticated(
        this ILogger logger,
        string userId);

    [LoggerMessage(
        EventId = 4002,
        Level = LogLevel.Warning,
        Message = "Authentication failed for user {UserId}")]
    public static partial void LogAuthenticationFailed(
        this ILogger logger,
        string userId);

    [LoggerMessage(
        EventId = 4003,
        Level = LogLevel.Warning,
        Message = "Authorization failed for user {UserId} accessing {Resource}")]
    public static partial void LogAuthorizationFailed(
        this ILogger logger,
        string userId,
        string resource);

    // File Operations Logging
    
    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Information,
        Message = "File upload started: {FileName} ({FileSizeBytes} bytes)")]
    public static partial void LogFileUploadStarted(
        this ILogger logger,
        string fileName,
        long fileSizeBytes);

    [LoggerMessage(
        EventId = 5002,
        Level = LogLevel.Information,
        Message = "File upload completed: {FileName} in {ElapsedMs} ms")]
    public static partial void LogFileUploadCompleted(
        this ILogger logger,
        string fileName,
        long elapsedMs);

    [LoggerMessage(
        EventId = 5003,
        Level = LogLevel.Error,
        Message = "File upload failed: {FileName}")]
    public static partial void LogFileUploadFailed(
        this ILogger logger,
        Exception exception,
        string fileName);

    // Cache Logging
    
    [LoggerMessage(
        EventId = 6001,
        Level = LogLevel.Debug,
        Message = "Cache hit for key {CacheKey}")]
    public static partial void LogCacheHit(
        this ILogger logger,
        string cacheKey);

    [LoggerMessage(
        EventId = 6002,
        Level = LogLevel.Debug,
        Message = "Cache miss for key {CacheKey}")]
    public static partial void LogCacheMiss(
        this ILogger logger,
        string cacheKey);

    // General Application Logging
    
    [LoggerMessage(
        EventId = 9001,
        Level = LogLevel.Information,
        Message = "Application started: {ApplicationName} v{Version}")]
    public static partial void LogApplicationStarted(
        this ILogger logger,
        string applicationName,
        string version);

    [LoggerMessage(
        EventId = 9002,
        Level = LogLevel.Information,
        Message = "Application stopping: {ApplicationName}")]
    public static partial void LogApplicationStopping(
        this ILogger logger,
        string applicationName);

    [LoggerMessage(
        EventId = 9999,
        Level = LogLevel.Critical,
        Message = "Unhandled exception occurred")]
    public static partial void LogUnhandledException(
        this ILogger logger,
        Exception exception);
}
