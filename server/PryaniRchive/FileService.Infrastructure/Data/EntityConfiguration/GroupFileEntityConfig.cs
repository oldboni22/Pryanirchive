using FileService.Domain.Entities;
using FileService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int BlobFileIdMaxLength = 500;
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

        builder.Property(f => f.FileBlobId)
            .HasConversion(
                v => v.Value,
                value => FileBlobId.FromDatabase(value))
            .HasMaxLength(FileBlobId.MaxLength);
        
        builder.HasOne(f => f.FileGroup)
            .WithMany(g => g.Files)
            .HasForeignKey(f => f.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(f => f.FileBlobId)
            .HasMaxLength(Constraints.BlobFileIdMaxLength);
        
        builder.HasIndex(f => new {f.GroupId, f.FileName, })
            .IsUnique();
        
        builder.HasIndex(f => new { f.OwnerId, f.GroupId });
    }
}
