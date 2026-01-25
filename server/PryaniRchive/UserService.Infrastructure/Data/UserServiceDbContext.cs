using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data;

public sealed class UserServiceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    
    public DbSet<UserGroupPermission>  UserPermissions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
    }
}
