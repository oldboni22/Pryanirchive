using Common.Logging;
using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace FileService.Infrastructure.Blob.MinIo;

file static class Constants
{
    public const string ContentDispositionHeaderName = "response-content-disposition";
    
    public const string InlineDisposition =  "inline";
    
    public const string AttachmentDisposition =  "attachment";
}

public abstract class MinIoBlobService(IMinioClient client, IOptions<MinIoBlobOptions> options, ILogger<MinIoBlobService> logger) : IBlobService
{
    protected abstract string BucketName { get; } 
        
    private readonly int _expirationSeconds = (int)TimeSpan.FromHours(options.Value.UrlExpireHours).TotalSeconds;
    
    public async Task<Result> UploadFileAsync(
        Stream fileStream, string fileBlobId, string contentType, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("Upload", fileBlobId, BucketName);
        
        try
        {
            await using (fileStream)
            {
                var putArgs = new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileBlobId)
                    .WithObjectSize(fileStream.Length)
                    .WithStreamData(fileStream)
                    .WithContentType(contentType);

                await client.PutObjectAsync(putArgs, cancellationToken);
                
                logger.LogBlobOperationCompleted("Upload", fileBlobId);
                
                return Result.Success();
            }
        }
        catch (Exception ex)
        {
            logger.LogBlobOperationFailed(ex, "Upload", fileBlobId);
            return ex;
        }
    }

    public async Task<Result<string>> GetFileLinkAsync(string fileBlobId, string fileName,bool isInline, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("GetLink", fileBlobId, BucketName);
        
        var dispositionType = isInline ? Constants.InlineDisposition : Constants.AttachmentDisposition;
        var encodedName = Uri.EscapeDataString(fileName);
        
        string contentDisposition = $"{dispositionType}; filename=\"{fileName}\"; filename*=UTF-8''{encodedName}";
        
        try
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileBlobId)
                .WithExpiry(_expirationSeconds)
                .WithHeaders(new Dictionary<string, string>
                {
                    {Constants.ContentDispositionHeaderName, contentDisposition}
                });
            
            var link = await client.PresignedGetObjectAsync(args);
            
            logger.LogBlobOperationCompleted("GetLink", fileBlobId);
            
            return link;
        }
        catch (Exception ex)
        {
            logger.LogBlobOperationFailed(ex, "GetLink", fileBlobId);
            return ex;
        }
    }

    public async Task<Result> DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("Delete", fileBlobId, BucketName);
        
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileBlobId);

            await client.RemoveObjectAsync(args, cancellationToken);
            
            logger.LogBlobOperationCompleted("Delete", fileBlobId);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogBlobOperationFailed(ex, "Delete", fileBlobId);
            return ex;
        }
    }

    public async Task<Result> EnsureStorageExists(CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("EnsureStorageExists", BucketName, BucketName);
        
        try
        {
            var existsArgs = new BucketExistsArgs().WithBucket(BucketName);
            var bucketExists = await client.BucketExistsAsync(existsArgs, cancellationToken);

            if (bucketExists)
            {
                logger.LogBlobOperationCompleted("EnsureStorageExists", BucketName);
                return Result.Success();
            }

            var createArgs = new MakeBucketArgs().WithBucket(BucketName);
            await client.MakeBucketAsync(createArgs, cancellationToken);
            
            logger.LogBlobOperationCompleted("EnsureStorageExists", BucketName);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogBlobOperationFailed(ex, "EnsureStorageExists", BucketName);
            return ex;
        }
    }
}
