using Common.Data;
using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure.Data;

public sealed class FileServiceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<FileGroup>  FileGroups { get; set; }
    
    public DbSet<GroupFile>  GroupFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileServiceDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddTimeStampInterceptor();
    }
}
