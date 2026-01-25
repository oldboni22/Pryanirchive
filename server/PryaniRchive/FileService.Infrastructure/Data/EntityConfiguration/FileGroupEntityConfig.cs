using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int MaxNameLength = 64;
}

public sealed class FileGroupEntityConfig : IEntityTypeConfiguration<FileGroup>
{
    public void Configure(EntityTypeBuilder<FileGroup> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Parent)
            .WithMany(x => x.NestedGroups)
            .HasForeignKey(x => x.ParentGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Constraints.MaxNameLength);

        builder.HasIndex(x => new{ x.OwnerId, x.Name})
            .HasFilter("\"ParentGroupId\" IS NULL")
            .IsUnique();
        
        builder.HasIndex(x => new { x.OwnerId, x.Name, x.ParentGroupId })
            .HasFilter("\"ParentGroupId\" IS NOT NULL")
            .IsUnique();
        
        builder.HasIndex(x => x.OwnerId)
            .HasFilter("\"ParentGroupId\" IS NULL");
        
        builder.HasIndex(x => new{ x.OwnerId, x.ParentGroupId });
    }
}
