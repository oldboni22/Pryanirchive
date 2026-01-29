using Common.Data;
using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure.Data;

public sealed class FileServiceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Folder>  Folders { get; set; }
    
    public DbSet<FileReference>  FileReferences { get; set; }

    public DbSet<Space> Spaces {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileServiceDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddTimeStampInterceptor();
    }
}
