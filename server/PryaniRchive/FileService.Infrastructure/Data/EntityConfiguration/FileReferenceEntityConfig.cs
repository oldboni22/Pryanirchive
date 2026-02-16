using FileService.Domain.Entities;
using FileService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Data.EntityConfiguration;

public sealed class FileReferenceEntityConfig : IEntityTypeConfiguration<FileReference>
{
    public void Configure(EntityTypeBuilder<FileReference> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FileName)
            .HasConversion(
                v => v.Value,
                value => FileName.FromDatabase(value))
            .HasMaxLength(FileName.MaxNameLength)
            .IsRequired();

        builder.Property(f => f.FileBlobId)
            .HasConversion(
                v => v.Value,
                value => FileBlobId.FromDatabase(value))
            .HasMaxLength(FileBlobId.MaxLength);
        
        builder
            .HasOne(f =>f.Space)
            .WithMany()
            .HasForeignKey(f => f.SpaceId);
        
        builder.HasOne(f => f.Folder)
            .WithMany(g => g.Files)
            .HasForeignKey(f => f.FolderId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(f => new { f.FolderId, f.FileName, })
            .IsUnique();
        
        builder.HasIndex(f => new { f.SpaceId, f.FolderId });
    }
}
