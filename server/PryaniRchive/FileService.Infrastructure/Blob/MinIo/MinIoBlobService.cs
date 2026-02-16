using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using Microsoft.Extensions.Logging;
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

public sealed class MinIoBlobService(
    IMinioClient client,
    MinIoServiceOptions options,
    ILogger<MinIoBlobService> logger) : IBlobService
{
    private readonly string _bucketName = options.BucketName;

    private readonly int _expirationSeconds = options.UrlExpireSeconds;

    public async Task<Result<FileUploadDto>> GetUploadLinkAsync(
        string fileBlobId, string contentType, long maxSize, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("GetUploadLink", fileBlobId, _bucketName);

        var policy = CreatePostPolicy(fileBlobId, contentType, maxSize);

        var (url, formData) = await client.PresignedPostPolicyAsync(policy);

        logger.LogBlobOperationCompleted("GetUploadLink", fileBlobId);

        return new FileUploadDto(url!.ToString(), new Dictionary<string, string>(formData!));
    }

    public async Task<Result<string>> GetLoadLinkAsync(
        string fileBlobId, string fileName, bool isInline, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("GetLink", fileBlobId, _bucketName);

        var dispositionType = isInline ? Constants.InlineDisposition : Constants.AttachmentDisposition;
        var encodedName = Uri.EscapeDataString(fileName);

        string contentDisposition = $"{dispositionType}; filename=\"{fileName}\"; filename*=UTF-8''{encodedName}";

        var args = new PresignedGetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileBlobId)
            .WithExpiry(_expirationSeconds)
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
        logger.LogBlobOperationStarted("Delete", fileBlobId, _bucketName);

        var args = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileBlobId);

        await client.RemoveObjectAsync(args, cancellationToken);

        logger.LogBlobOperationCompleted("Delete", fileBlobId);

        return Result.Success();
    }

    public async Task<Result> EnsureStorageExists(CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("EnsureStorageExists", _bucketName, _bucketName);

        var existsArgs = new BucketExistsArgs().WithBucket(_bucketName);
        var bucketExists = await client.BucketExistsAsync(existsArgs, cancellationToken);

        if (bucketExists)
        {
            logger.LogBlobOperationCompleted("EnsureStorageExists", _bucketName);
            return Result.Success();
        }

        var createArgs = new MakeBucketArgs().WithBucket(_bucketName);
        await client.MakeBucketAsync(createArgs, cancellationToken);

        logger.LogBlobOperationCompleted("EnsureStorageExists", _bucketName);

        return Result.Success();
    }

    private PostPolicy CreatePostPolicy(string objectName, string contentType, long maxSize)
    {
        var policy = new PostPolicy();

        policy.SetBucket(_bucketName);
        policy.SetKey(objectName);
        policy.SetExpires(DateTime.UtcNow.AddSeconds(_expirationSeconds));

        policy.SetContentRange(1, maxSize);

        policy.SetContentType(contentType);

        return policy;
    }
}