using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface ICachedFileService
{
    Task<Result<string>> GetAsync(string key, string fileName, bool isInline, CancellationToken cancellationToken = default);
    
    Task<Result> SetAsync(string key, Stream value, string contentType, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveAsync(string key, CancellationToken cancellationToken = default);
}
