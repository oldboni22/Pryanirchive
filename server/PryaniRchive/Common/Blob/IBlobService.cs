namespace Common.Blob;

public interface IBlobService
{
    Task<string?> UploadFileAsync(Stream fileStream, string fileBlobId, string contentType, CancellationToken cancellationToken = default);
    
    Task<string?> GetFileLinkAsync(string fileBlobId, CancellationToken cancellationToken = default);
    
    Task<FileOutput?> GetFileAsync(string fileBlobId, CancellationToken cancellationToken = default);
    
    Task DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default);

    Task EnsureStorageExists(CancellationToken cancellationToken = default);
}
