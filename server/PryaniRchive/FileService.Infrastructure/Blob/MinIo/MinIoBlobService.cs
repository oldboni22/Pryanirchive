using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
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

public abstract class MinIoBlobService(IMinioClient client, IOptions<MinIoBlobOptions> options) : IBlobService
{
    protected abstract string BucketName { get; } 
        
    private readonly int _expirationSeconds = (int)TimeSpan.FromHours(options.Value.UrlExpireHours).TotalSeconds;
    
    public async Task<Result> UploadFileAsync(
        Stream fileStream, string fileBlobId, string contentType, CancellationToken cancellationToken = default)
    {
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
                
                return Result.Success();
            }
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result<string>> GetFileLinkAsync(string fileBlobId, string fileName,bool isInline, CancellationToken cancellationToken = default)
    {
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
            return link;
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
                .WithBucket(BucketName)
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
            var existsArgs = new BucketExistsArgs().WithBucket(BucketName);
            var bucketExists = await client.BucketExistsAsync(existsArgs, cancellationToken);

            if (bucketExists) return Result.Success();

            var createArgs = new MakeBucketArgs().WithBucket(BucketName);
            await client.MakeBucketAsync(createArgs, cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
