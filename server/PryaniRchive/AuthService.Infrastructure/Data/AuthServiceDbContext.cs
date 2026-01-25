using AuthService.Domain.Entities;
using Common.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data;

public class AuthServiceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AuthenticationCredentials>  AuthenticationCredentials { get; init; }
    
    public DbSet<UserSession> UserSessions { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddTimeStampInterceptor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(AuthServiceDbContext).Assembly);
    }
}
