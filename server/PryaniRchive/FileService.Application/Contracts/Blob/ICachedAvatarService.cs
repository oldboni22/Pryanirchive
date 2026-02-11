using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface ICachedAvatarService
{
    Task<Result<string>> GetLoadLinkAsync(Guid userId, string key, CancellationToken cancellationToken = default);
    
    Task<Result<FileUploadDto>> GetUploadLinkAsync(Guid userId, string key, string contentType, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveAsync(Guid userId, string key, CancellationToken cancellationToken = default);
}
