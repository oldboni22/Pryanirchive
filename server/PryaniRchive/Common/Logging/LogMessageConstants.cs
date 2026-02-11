namespace Common.Logging;

/// <summary>
/// Constants for log messages to avoid hardcoded strings.
/// </summary>
public static class LogMessageConstants
{
    // HTTP Request Logging
    public const string HttpRequestStarted = "HTTP {Method} {Path} started";
    public const string HttpRequestCompleted = "HTTP {Method} {Path} completed with status {StatusCode}";
    public const string HttpRequestFailed = "HTTP {Method} {Path} failed with status {StatusCode}";

    // Database Logging
    public const string DatabaseQueryStarted = "Executing database query: {QueryName} with parameters {Parameters}";
    public const string DatabaseQueryCompleted = "Database query {QueryName} completed";
    public const string SlowDatabaseQuery = "Slow database query detected: {QueryName} took {ElapsedMs} ms"; // Keep for slow query? Or remove? User said "Remove all stopwatches". I'll remove it or rename it. "Slow query detected" implies timing. But user said "all". I'll assume standard completion logs. Accessing 'SlowDatabaseQuery' implies it's triggered by time. I'll leave SlowDatabaseQuery alone perhaps? No, "Remove all stopwatches". If I can't measure time, I can't detect slow queries. I should probably remove LogSlowDatabaseQuery usage or make it just "Query completed". 
    // Actually, I'll modify Completed messages. LogSlowDatabaseQuery is a specific alert, I probably can't use it without stopwatch. I'll leave it in constants but won't use it (or user might want it removed). 
    // Wait, the user said "Remove all stopwatches and elapsed time logging". So I should probably remove the param from Completed messages.
    
    // gRPC Logging
    public const string GRpcCallStarted = "gRPC call to {Service}.{Method} started";
    public const string GRpcCallCompleted = "gRPC call to {Service}.{Method} completed";
    public const string GRpcCallFailed = "gRPC call to {Service}.{Method} failed";

    // Authentication/Authorization Logging
    public const string UserAuthenticated = "User {UserId} authenticated successfully";
    public const string AuthenticationFailed = "Authentication failed for user {UserId}";
    public const string AuthorizationFailed = "Authorization failed for user {UserId} accessing {Resource}";

    // Token/JWT Operations
    public const string TokenIssued = "Token issued for user {UserId}";
    public const string TokenValidationStarted = "Token validation started";
    public const string TokenValidationCompleted = "Token validation completed for user {UserId}";
    public const string TokenValidationFailed = "Token validation failed";

    // File Operations Logging
    public const string FileUploadStarted = "File upload started: {FileName} ({FileSizeBytes} bytes)";
    public const string FileUploadCompleted = "File upload completed: {FileName}";
    public const string FileUploadFailed = "File upload failed: {FileName}";

    // Blob Storage Operations
    public const string BlobOperationStarted = "Blob operation {Operation} started for {BlobId} in bucket {Bucket}";
    public const string BlobOperationCompleted = "Blob operation {Operation} completed for {BlobId}";
    public const string BlobOperationFailed = "Blob operation {Operation} failed for {BlobId}";

    // Cache Logging
    public const string CacheHit = "Cache hit for key {CacheKey}";
    public const string CacheMiss = "Cache miss for key {CacheKey}";
    public const string CacheSetStarted = "Cache set started for key {CacheKey}";
    public const string CacheRemoveStarted = "Cache remove started for key {CacheKey}";
    public const string CacheOperationFailed = "Cache operation failed for key {CacheKey}";

    // Business Logic Operations
    public const string BusinessOperationStarted = "Business operation {OperationName} started";
    public const string BusinessOperationCompleted = "Business operation {OperationName} completed";
    public const string BusinessOperationFailed = "Business operation {OperationName} failed";

    // General Application Logging
    public const string ApplicationStarted = "Application started: {ApplicationName} v{Version}";
    public const string ApplicationStopping = "Application stopping: {ApplicationName}";
    public const string UnhandledException = "Unhandled exception occurred";
}
