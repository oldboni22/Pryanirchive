using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int MaxNameLength = 25;
}

public class SpaceEntityConfig : IEntityTypeConfiguration<Space>
{
    public void Configure(EntityTypeBuilder<Space> builder)
    { 
        builder.HasKey(x => x.Id);
        
        builder
            .HasIndex(x => new {x.OwnerId, x.Name})
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(Constraints.MaxNameLength);
    }
}
