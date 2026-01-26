namespace Common.Blob;

public interface IBlobService
{
    Task<bool> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    
    Task<string?> GetFileLinkAsync(string fileName);
    
    Task<FileOutput?> GetFileAsync(string fileName);
    
    Task DeleteFileAsync(string fileName);

    Task EnsureStorageExists();
}
