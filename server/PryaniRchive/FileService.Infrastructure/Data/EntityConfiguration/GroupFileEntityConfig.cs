using FileService.Domain.Entities;
using FileService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int FileUrlMaxLength = 500;
}

public sealed class GroupFileEntityConfig : IEntityTypeConfiguration<GroupFile>
{
    public void Configure(EntityTypeBuilder<GroupFile> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FileName)
            .HasConversion(
                v => v.Value,
                value => FileName.FromDatabase(value))
            .HasMaxLength(FileName.MaxFileNameLength)
            .IsRequired();
        
        builder.HasOne(f => f.FileGroup)
            .WithMany(g => g.Files)
            .HasForeignKey(f => f.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(f => f.FileUrl)
            .HasMaxLength(Constraints.FileUrlMaxLength);
        
        builder.HasIndex(f => new {f.GroupId, f.FileName, })
            .IsUnique();
        
        builder.HasIndex(f => new { f.OwnerId, f.GroupId });
    }
}
