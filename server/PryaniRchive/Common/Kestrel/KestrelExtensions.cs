using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace Common.Kestrel;

public static class KestrelExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public void ConfigureKestrel(bool mapGRpc)
        {
            var kestrelOptions = builder.Configuration.GetSection(KestrelOptions.ConfigurationSection).Get<KestrelOptions>()!;

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(kestrelOptions.ApiPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                });

                if (!mapGRpc)
                {
                    return;
                }
                
                if (kestrelOptions.LocalNetworkIp is null)
                {
                    throw new InvalidConfigurationException();
                }
                
                options.Listen(IPAddress.Parse(kestrelOptions.LocalNetworkIp), kestrelOptions.GRpcPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });
        }
    }
}