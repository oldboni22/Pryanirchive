using FileService.Application.Contracts.Blob;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure.Blob.CachedServices;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {   
        public IServiceCollection RegisterCachedAvatar()
        {
            return services
                .AddSingleton<ICachedAvatarService, CachedAvatarService>();
        }
    }

}