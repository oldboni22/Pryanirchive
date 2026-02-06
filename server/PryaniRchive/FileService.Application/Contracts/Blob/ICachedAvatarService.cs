namespace FileService.Application.Contracts.Blob;

public interface ICachedAvatarService
{
    Task<string> GetAsync(string key, CancellationToken cancellationToken = default);
    
    Task SetAsync(string key, Stream value, string contentType, CancellationToken cancellationToken = default);
    
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
