using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Common.Logging;

public static class LoggingExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddSerilogLogging()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .CreateLogger();

            // Replace default logging with Serilog
            builder.Host.UseSerilog();

            return builder;
        }
    }

    extension(WebApplication app)
    {
        public WebApplication UseSerilogRequestLogging()
        {
            // Add Serilog request logging middleware
            // This logs HTTP requests with detailed information
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.GetLevel = (httpContext, elapsed, ex) => ex != null 
                    ? LogEventLevel.Error 
                    : elapsed > 1000 
                        ? LogEventLevel.Warning 
                        : LogEventLevel.Information;
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                    
                    if (httpContext.Items.TryGetValue("CorrelationId", out var correlationId))
                    {
                        diagnosticContext.Set("CorrelationId", correlationId);
                    }
                };
            });

            return app;
        }
    }
}
