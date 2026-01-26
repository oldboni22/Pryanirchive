using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Common.Blob.MinIo;

public class MinIoBlobService(IMinioClient client, IOptions<MinIoBlobOptions> options) : IBlobService
{
    private readonly string _bucketName = options.Value.BucketName;
    
    private readonly int _expirationSeconds = (int)TimeSpan.FromHours(options.Value.UrlExpireHours).TotalSeconds;
    
    public async Task<string?> UploadFileAsync(
        Stream fileStream, string fileBlobId, string contentType, CancellationToken cancellationToken = default)
    {
        await using (fileStream)
        {
            var putArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileBlobId)
                .WithObjectSize(fileStream.Length)
                .WithStreamData(fileStream)
                .WithContentType(contentType);

            await client.PutObjectAsync(putArgs, cancellationToken);

            return await GetFileLinkAsync(fileBlobId, cancellationToken);
        }
    }


    public async Task<string?> GetFileLinkAsync(string fileBlobId, CancellationToken cancellationToken = default)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileBlobId)
            .WithExpiry(_expirationSeconds);
        
        return await client.PresignedGetObjectAsync(args);
    }

    public async Task<FileOutput?> GetFileAsync(string fileBlobId, CancellationToken cancellationToken = default)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileBlobId)
                .WithCallbackStream(async (stream, ct) =>
                {
                    await stream.CopyToAsync(memoryStream, ct);
                });
            
            var stats = await client.GetObjectAsync(args, cancellationToken);
            
            memoryStream.Position = 0;

            return new FileOutput
            {
                Content = memoryStream,
                ContentType = stats.ContentType,
                FileName = fileBlobId // Или оригинальное имя, если оно сохранено отдельно
            };
        }
        catch (ObjectNotFoundException)
        {
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default)
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileBlobId);

            await client.RemoveObjectAsync(args, cancellationToken);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task EnsureStorageExists(CancellationToken cancellationToken = default)
    {
        try
        {
            var existsArgs = new BucketExistsArgs()
                .WithBucket(_bucketName);

            var bucketExists = await client.BucketExistsAsync(existsArgs, cancellationToken);

            if (bucketExists)
            {
                return;
            }

            var createArgs = new MakeBucketArgs()
                .WithBucket(_bucketName);

            await client.MakeBucketAsync(createArgs, cancellationToken);
        }
        catch(MinioException )
        {
            throw;
        }
    }
}
