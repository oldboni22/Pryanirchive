using FileService.Application.Contracts.Blob;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Application;

public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        public async Task EnsureBlobExists()
        {
            using var scope = app.Services.CreateScope();
            
            var blobService = scope.ServiceProvider.GetService<IBlobService>()
                ?? throw new NullReferenceException("Blob service not found.");

            await blobService.EnsureStorageExists();
        }
    }
}