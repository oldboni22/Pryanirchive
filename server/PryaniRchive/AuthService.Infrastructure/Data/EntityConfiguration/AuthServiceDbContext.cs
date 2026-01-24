using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.EntityConfiguration;

public class AuthServiceDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthServiceDbContext).Assembly);
    }
}

