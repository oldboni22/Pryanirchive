using Microsoft.Extensions.Logging;

namespace Common.Logging;

public static partial class LogMessages
{
    // HTTP Request Logging
    
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = LogMessageConstants.HttpRequestStarted)]
    public static partial void LogHttpRequestStarted(
        this ILogger logger,
        string method,
        string path);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Information,
        Message = LogMessageConstants.HttpRequestCompleted)]
    public static partial void LogHttpRequestCompleted(
        this ILogger logger,
        string method,
        string path,
        int statusCode);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Error,
        Message = LogMessageConstants.HttpRequestFailed)]
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
        Message = LogMessageConstants.DatabaseQueryStarted)]
    public static partial void LogDatabaseQueryStarted(
        this ILogger logger,
        string queryName,
        string parameters);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Information,
        Message = LogMessageConstants.DatabaseQueryCompleted)]
    public static partial void LogDatabaseQueryCompleted(
        this ILogger logger,
        string queryName);

    [LoggerMessage(
        EventId = 2003,
        Level = LogLevel.Warning,
        Message = LogMessageConstants.SlowDatabaseQuery)]
    public static partial void LogSlowDatabaseQuery(
        this ILogger logger,
        string queryName,
        long elapsedMs);

    // gRPC Logging
    
    [LoggerMessage(
        EventId = 3001,
        Level = LogLevel.Information,
        Message = LogMessageConstants.GRpcCallStarted)]
    public static partial void LogGRpcCallStarted(
        this ILogger logger,
        string service,
        string method);

    [LoggerMessage(
        EventId = 3002,
        Level = LogLevel.Information,
        Message = LogMessageConstants.GRpcCallCompleted)]
    public static partial void LogGRpcCallCompleted(
        this ILogger logger,
        string service,
        string method);

    [LoggerMessage(
        EventId = 3003,
        Level = LogLevel.Error,
        Message = LogMessageConstants.GRpcCallFailed)]
    public static partial void LogGRpcCallFailed(
        this ILogger logger,
        Exception exception,
        string service,
        string method);

    // Authentication/Authorization Logging
    
    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = LogMessageConstants.UserAuthenticated)]
    public static partial void LogUserAuthenticated(
        this ILogger logger,
        string userId);

    [LoggerMessage(
        EventId = 4002,
        Level = LogLevel.Warning,
        Message = LogMessageConstants.AuthenticationFailed)]
    public static partial void LogAuthenticationFailed(
        this ILogger logger,
        string userId);

    [LoggerMessage(
        EventId = 4003,
        Level = LogLevel.Warning,
        Message = LogMessageConstants.AuthorizationFailed)]
    public static partial void LogAuthorizationFailed(
        this ILogger logger,
        string userId,
        string resource);

    // File Operations Logging
    
    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Information,
        Message = LogMessageConstants.FileUploadStarted)]
    public static partial void LogFileUploadStarted(
        this ILogger logger,
        string fileName,
        long fileSizeBytes);

    [LoggerMessage(
        EventId = 5002,
        Level = LogLevel.Information,
        Message = LogMessageConstants.FileUploadCompleted)]
    public static partial void LogFileUploadCompleted(
        this ILogger logger,
        string fileName);

    [LoggerMessage(
        EventId = 5003,
        Level = LogLevel.Error,
        Message = LogMessageConstants.FileUploadFailed)]
    public static partial void LogFileUploadFailed(
        this ILogger logger,
        Exception exception,
        string fileName);

    // Cache Logging
    
    [LoggerMessage(
        EventId = 6001,
        Level = LogLevel.Debug,
        Message = LogMessageConstants.CacheHit)]
    public static partial void LogCacheHit(
        this ILogger logger,
        string cacheKey);

    [LoggerMessage(
        EventId = 6002,
        Level = LogLevel.Debug,
        Message = LogMessageConstants.CacheMiss)]
    public static partial void LogCacheMiss(
        this ILogger logger,
        string cacheKey);

    [LoggerMessage(
        EventId = 6003,
        Level = LogLevel.Debug,
        Message = LogMessageConstants.CacheSetStarted)]
    public static partial void LogCacheSetStarted(
        this ILogger logger,
        string cacheKey);

    [LoggerMessage(
        EventId = 6004,
        Level = LogLevel.Debug,
        Message = LogMessageConstants.CacheRemoveStarted)]
    public static partial void LogCacheRemoveStarted(
        this ILogger logger,
        string cacheKey);

    [LoggerMessage(
        EventId = 6005,
        Level = LogLevel.Error,
        Message = LogMessageConstants.CacheOperationFailed)]
    public static partial void LogCacheOperationFailed(
        this ILogger logger,
        Exception exception,
        string cacheKey);

    // Token/JWT Operations
    
    [LoggerMessage(
        EventId = 4004,
        Level = LogLevel.Information,
        Message = LogMessageConstants.TokenIssued)]
    public static partial void LogTokenIssued(
        this ILogger logger,
        Guid userId);

    [LoggerMessage(
        EventId = 4005,
        Level = LogLevel.Debug,
        Message = LogMessageConstants.TokenValidationStarted)]
    public static partial void LogTokenValidationStarted(
        this ILogger logger);

    [LoggerMessage(
        EventId = 4006,
        Level = LogLevel.Information,
        Message = LogMessageConstants.TokenValidationCompleted)]
    public static partial void LogTokenValidationCompleted(
        this ILogger logger,
        Guid userId);

    [LoggerMessage(
        EventId = 4007,
        Level = LogLevel.Warning,
        Message = LogMessageConstants.TokenValidationFailed)]
    public static partial void LogTokenValidationFailed(
        this ILogger logger,
        Exception exception);

    // Blob Storage Operations
    
    [LoggerMessage(
        EventId = 5004,
        Level = LogLevel.Information,
        Message = LogMessageConstants.BlobOperationStarted)]
    public static partial void LogBlobOperationStarted(
        this ILogger logger,
        string operation,
        string blobId,
        string bucket);

    [LoggerMessage(
        EventId = 5005,
        Level = LogLevel.Information,
        Message = LogMessageConstants.BlobOperationCompleted)]
    public static partial void LogBlobOperationCompleted(
        this ILogger logger,
        string operation,
        string blobId);

    [LoggerMessage(
        EventId = 5006,
        Level = LogLevel.Error,
        Message = LogMessageConstants.BlobOperationFailed)]
    public static partial void LogBlobOperationFailed(
        this ILogger logger,
        Exception exception,
        string operation,
        string blobId);

    // Business Logic Operations
    
    [LoggerMessage(
        EventId = 7001,
        Level = LogLevel.Information,
        Message = LogMessageConstants.BusinessOperationStarted)]
    public static partial void LogBusinessOperationStarted(
        this ILogger logger,
        string operationName);

    [LoggerMessage(
        EventId = 7002,
        Level = LogLevel.Information,
        Message = LogMessageConstants.BusinessOperationCompleted)]
    public static partial void LogBusinessOperationCompleted(
        this ILogger logger,
        string operationName);

    [LoggerMessage(
        EventId = 7003,
        Level = LogLevel.Error,
        Message = LogMessageConstants.BusinessOperationFailed)]
    public static partial void LogBusinessOperationFailed(
        this ILogger logger,
        Exception exception,
        string operationName);

    // General Application Logging
    
    [LoggerMessage(
        EventId = 9001,
        Level = LogLevel.Information,
        Message = LogMessageConstants.ApplicationStarted)]
    public static partial void LogApplicationStarted(
        this ILogger logger,
        string applicationName,
        string version);

    [LoggerMessage(
        EventId = 9002,
        Level = LogLevel.Information,
        Message = LogMessageConstants.ApplicationStopping)]
    public static partial void LogApplicationStopping(
        this ILogger logger,
        string applicationName);

    [LoggerMessage(
        EventId = 9999,
        Level = LogLevel.Critical,
        Message = LogMessageConstants.UnhandledException)]
    public static partial void LogUnhandledException(
        this ILogger logger,
        Exception exception);
}
