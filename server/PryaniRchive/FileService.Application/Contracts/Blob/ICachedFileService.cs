using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface ICachedFileService
{
    Task<Result<string>> GetLoadLinkAsync(string key, string fileName, bool isInline, CancellationToken cancellationToken = default);
    
    Task<Result<FileUploadDto>> GetUploadLinkAsync(string key, string contentType, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveAsync(string key, CancellationToken cancellationToken = default);
}
