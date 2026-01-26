using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Common.Authentication;

public static class AuthenticationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJwtAuthentication(IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(JwtServiceOptions.SectionName).Get<JwtServiceOptions>()!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignatureKey)),
                        
                        ValidateLifetime = true,
                    };
                });

            return services;
        }
    } 
}
