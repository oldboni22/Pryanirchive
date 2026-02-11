using FileService.Application.Contracts.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMinIoBlobService(IConfiguration configuration)
        {
            services
                .Configure<MinIoConnectionOptions>(configuration.GetSection(MinIoConnectionOptions.ConfigSection))
                .Configure<MinIoServiceOptions>(
                    MinIoServiceOptions.AvatarKey, configuration.GetSection(MinIoServiceOptions.AvatarSection))
                .Configure<MinIoServiceOptions>(
                    MinIoServiceOptions.FileKey, configuration.GetSection(MinIoServiceOptions.FileSection));
            
            var options = configuration.GetSection(MinIoConnectionOptions.ConfigSection).Get<MinIoConnectionOptions>()!;
            
            services.AddMinio(configureAction =>
            {
                configureAction
                    .WithEndpoint(options.Endpoint)
                    .WithCredentials(options.AccessKey, options.SecretKey)
                    .WithSSL(false)
                    .Build();
            });
            
            return 
                services
                    .AddKeyedSingleton<IBlobService, AvatarMinioService>(AvatarMinioService.Key)
                    .AddKeyedSingleton<IBlobService, FileMinioService>(FileMinioService.Key);
        }
    }
}
