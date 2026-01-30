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
            services.Configure<MinIoBlobOptions>(configuration.GetSection(MinIoBlobOptions.ConfigSection));
            
            var options = configuration.GetSection(MinIoBlobOptions.ConfigSection).Get<MinIoBlobOptions>()!;
            
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
