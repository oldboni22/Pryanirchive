using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface ICachedAvatarService
{
    Task<Result<string>> GetAsync(string key, CancellationToken cancellationToken = default);
    
    Task<Result> SetAsync(string key, Stream value, string contentType, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveAsync(string key, CancellationToken cancellationToken = default);
}
