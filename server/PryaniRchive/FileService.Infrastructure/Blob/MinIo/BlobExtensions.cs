using FileService.Application.Contracts.Blob;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure.Blob.MinIo;

public static class BlobExtensions
{
    extension(WebApplication app)
    {
        public async Task EnsureBlobExists()
        {
            var avatarService = app.Services.GetKeyedService<IBlobService>(AvatarMinioService.Key)
                ?? throw new NullReferenceException("Blob service not found.");

            var fileService = app.Services.GetKeyedService<IBlobService>(AvatarMinioService.Key)
                                ?? throw new NullReferenceException("Blob service not found.");
            
            await Task.WhenAll(
                fileService.EnsureStorageExists(),  
                avatarService.EnsureStorageExists());
        }
    }
}
