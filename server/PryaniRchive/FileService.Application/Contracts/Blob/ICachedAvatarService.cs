using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface ICachedAvatarService
{
    Task<Result<string>> GetAsync(Guid userId, string key, CancellationToken cancellationToken = default);
    
    Task<Result<string>> SetAsync(Guid userId, string key, Stream value, string contentType, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveAsync(Guid userId, string key, CancellationToken cancellationToken = default);
}
