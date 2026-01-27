using Common.Blob.MinIo;
using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using FileService.Domain;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace FileService.Infrastructure.Blob.MinIo;

public class MinIoBlobService(IMinioClient client, IOptions<MinIoBlobOptions> options) : IBlobService
{
    private readonly string _bucketName = options.Value.BucketName;
    private readonly int _expirationSeconds = (int)TimeSpan.FromHours(options.Value.UrlExpireHours).TotalSeconds;
    
    public async Task<Result<string>> UploadFileAsync(
        Stream fileStream, string fileBlobId, string contentType, CancellationToken cancellationToken = default)
    {
        try
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
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result<string>> GetFileLinkAsync(string fileBlobId, CancellationToken cancellationToken = default)
    {
        try
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileBlobId)
                .WithExpiry(_expirationSeconds);
            
            var link = await client.PresignedGetObjectAsync(args);
            return link;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result<FileOutput>> GetFileAsync(string fileBlobId, CancellationToken cancellationToken = default)
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
                FileName = fileBlobId
            };
        }
        catch (ObjectNotFoundException)
        {
            return DomainErrors.BlobNotFound;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result> DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default)
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileBlobId);

            await client.RemoveObjectAsync(args, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result> EnsureStorageExists(CancellationToken cancellationToken = default)
    {
        try
        {
            var existsArgs = new BucketExistsArgs().WithBucket(_bucketName);
            var bucketExists = await client.BucketExistsAsync(existsArgs, cancellationToken);

            if (bucketExists) return Result.Success();

            var createArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await client.MakeBucketAsync(createArgs, cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
