using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Data;

public static class DataExtensions
{
    extension(WebApplication app)
    {
        public async Task MigrateAsync<TContext>() where TContext : DbContext 
        {
            using var scope = app.Services.CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            
            await context.Database.MigrateAsync();
        }
    }
    
    extension(DbContextOptionsBuilder optionsBuilder)
    {
        public DbContextOptionsBuilder AddTimeStampInterceptor()
        {
            return optionsBuilder
                .AddInterceptors(new TimeStampInterceptor());
        }
    }
}
