using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.EntityConfiguration;

public class AuthServiceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AuthenticationCredentials>  AuthenticationCredentials { get; set; }
    
    public DbSet<UserSession> UserSessions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthServiceDbContext).Assembly);
    }
}
