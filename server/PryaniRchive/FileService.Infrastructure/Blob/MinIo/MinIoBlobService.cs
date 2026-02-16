using Common.Logging;
using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace FileService.Infrastructure.Blob.MinIo;

file static class Constants
{
    public const string ContentDispositionHeaderName = "response-content-disposition";

    public const string InlineDisposition = "inline";

    public const string AttachmentDisposition = "attachment";
}

public abstract class MinIoBlobService(
    IMinioClient client,
    IOptions<MinIoConnectionOptions> options,
    ILogger<MinIoBlobService> logger) : IBlobService
{
    protected abstract string BucketName { get; }

    protected abstract int ExpirationSeconds { get; }

    public async Task<Result<FileUploadDto>> GetUploadLinkAsync(
        string fileBlobId, string contentType, long maxSize, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("GetUploadLink", fileBlobId, BucketName);

        var policy = CreatePostPolicy(fileBlobId, contentType, maxSize);

        var (url, formData) = await client.PresignedPostPolicyAsync(policy);

        logger.LogBlobOperationCompleted("GetUploadLink", fileBlobId);

        return new FileUploadDto(url!.ToString(), new Dictionary<string, string>(formData!));
    }

    public async Task<Result<string>> GetLoadLinkAsync(
        string fileBlobId, string fileName, bool isInline, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("GetLink", fileBlobId, BucketName);

        var dispositionType = isInline ? Constants.InlineDisposition : Constants.AttachmentDisposition;
        var encodedName = Uri.EscapeDataString(fileName);

        string contentDisposition = $"{dispositionType}; filename=\"{fileName}\"; filename*=UTF-8''{encodedName}";

        var args = new PresignedGetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(fileBlobId)
            .WithExpiry(ExpirationSeconds)
            .WithHeaders(new Dictionary<string, string>
            {
                { Constants.ContentDispositionHeaderName, contentDisposition }
            });

        var link = await client.PresignedGetObjectAsync(args);

        logger.LogBlobOperationCompleted("GetLink", fileBlobId);

        return link;
    }

    public async Task<Result> DeleteFileAsync(string fileBlobId, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("Delete", fileBlobId, BucketName);

        var args = new RemoveObjectArgs()
            .WithBucket(BucketName)
            .WithObject(fileBlobId);

        await client.RemoveObjectAsync(args, cancellationToken);

        logger.LogBlobOperationCompleted("Delete", fileBlobId);

        return Result.Success();
    }

    public async Task<Result> EnsureStorageExists(CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("EnsureStorageExists", BucketName, BucketName);

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

    private PostPolicy CreatePostPolicy(string objectName, string contentType, long maxSize)
    {
        var policy = new PostPolicy();

        policy.SetBucket(BucketName);
        policy.SetKey(objectName);
        policy.SetExpires(DateTime.UtcNow.AddSeconds(ExpirationSeconds));

        policy.SetContentRange(1, maxSize);

        policy.SetContentType(contentType);

        return policy;
    }
}