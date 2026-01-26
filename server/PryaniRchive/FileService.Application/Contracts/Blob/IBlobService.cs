using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public interface IBlobService
{
    Task<Result<string>> UploadFileAsync(Stream fileStream, string fileBlobId, string contentType, CancellationToken cancellationToken = default);
    
    Task<Result<string>> GetFileLinkAsync(string fileBlobId, CancellationToken cancellationToken = default);
    
    Task<Result<FileOutput>> GetFileAsync(string fileBlobId, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default);

    Task<Result> EnsureStorageExists(CancellationToken cancellationToken = default);
}
