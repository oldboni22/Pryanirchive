using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

namespace Common;

public static class OpenApiExtensions
{
    /// <summary>
    /// Add open api to service collection before calling
    /// </summary>
    extension(WebApplication app)
    {
        public WebApplication MapScalar()
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
            
            return app;
        }
    }
}
