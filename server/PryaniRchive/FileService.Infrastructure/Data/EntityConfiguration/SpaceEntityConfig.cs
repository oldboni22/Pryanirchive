using FileService.Domain.Entities;
using FileService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Data.EntityConfiguration;

public class SpaceEntityConfig : IEntityTypeConfiguration<Space>
{
    public void Configure(EntityTypeBuilder<Space> builder)
    { 
        builder.HasKey(x => x.Id);
        
        builder
            .HasIndex(x => new {x.OwnerId, x.Name})
            .IsUnique();

        builder.Property(x => x.Name)
            .HasConversion(
                v => v.ToString(),
                value => SpaceName.FromDatabase(value))
            .IsRequired()
            .HasMaxLength(Space.MaxNameLength);
    }
}
