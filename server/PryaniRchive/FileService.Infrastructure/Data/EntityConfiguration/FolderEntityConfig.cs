using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int MaxNameLength = 64;
}

public sealed class FolderEntityConfig : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Parent)
            .WithMany(x => x.NestedFolders)
            .HasForeignKey(x => x.ParentFolderId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(f => f.Space)
            .WithMany()
            .HasForeignKey(f => f.SpaceId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Constraints.MaxNameLength);
        
        builder.HasIndex(x => new{ x.SpaceId, x.Name})
            .HasFilter("\"ParentFolderId\" IS NULL")
            .IsUnique();
        
        builder.HasIndex(x => new { x.SpaceId, x.Name, x.ParentFolderId })
            .HasFilter("\"ParentFolderId\" IS NOT NULL")
            .IsUnique();
        
        builder.HasIndex(x => x.SpaceId)
            .HasFilter("\"ParentFolderId\" IS NULL");
        
        builder.HasIndex(x => new{ x.SpaceId, x.ParentFolderId });
    }
}
