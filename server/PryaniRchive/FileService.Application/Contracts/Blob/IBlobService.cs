using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface IBlobService
{
    Task<Result<FileUploadDto>> GetUploadLinkAsync(string fileBlobId, string contentType, long maxSize, CancellationToken cancellationToken = default);
    
    Task<Result<string>> GetLoadLinkAsync(string fileBlobId, string fileName, bool isInline, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default);

    Task<Result> EnsureStorageExists(CancellationToken cancellationToken = default);
}
