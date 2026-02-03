using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface IBlobService
{
    Task<Result> UploadFileAsync(Stream fileStream, string fileBlobId, string contentType, CancellationToken cancellationToken = default);
    
    Task<Result<string>> GetFileLinkAsync(string fileBlobId, string fileName, bool isInline, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default);

    Task<Result> EnsureStorageExists(CancellationToken cancellationToken = default);
}
