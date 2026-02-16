using System;
using FileService.Application.Contracts.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMinIoBlobService(IConfiguration configuration)
        {
            services
                .Configure<MinIoConnectionOptions>(configuration.GetSection(MinIoConnectionOptions.ConfigSection));
            
            var options = configuration.GetSection(MinIoConnectionOptions.ConfigSection).Get<MinIoConnectionOptions>()!;
            
            services.AddMinio(configureAction =>
            {
                configureAction
                    .WithEndpoint(options.Endpoint)
                    .WithCredentials(options.AccessKey, options.SecretKey)
                    .WithSSL(false)
                    .Build();
            });

            return services
                .RegisterMinIoService(configuration, BlobDiKeys.AvatarKey, MinIoServiceOptions.AvatarSection)
                .RegisterMinIoService(configuration, BlobDiKeys.FileKey, MinIoServiceOptions.FileSection);
        }

        private IServiceCollection RegisterMinIoService(IConfiguration configuration, object key, string configSection)
        {
            return services.AddKeyedSingleton<IBlobService>(key, (sp, _) =>
            {
                var options = configuration.GetSection(configSection).Get<MinIoServiceOptions>() 
                              ?? throw new InvalidConfigurationException();
                
                var client = sp.GetRequiredService<IMinioClient>() 
                             ?? throw new InvalidOperationException();
                var logger = sp.GetRequiredService<ILogger<MinIoBlobService>>() 
                             ?? throw new InvalidOperationException();
                
                return new MinIoBlobService(client, options, logger);
            });
        }
    }
}
