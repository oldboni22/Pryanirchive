using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Application.Contracts.Blob;

public static class BlobExtensions
{
    extension(WebApplication app)
    {
        public async Task EnsureBlobExists()
        {
            var blobService = app.Services.GetService<IBlobService>()
                ?? throw new NullReferenceException("Blob service not found.");

            await blobService.EnsureStorageExists();
        }
    }
}
