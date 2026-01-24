using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int MaxHashLength = 128;
    
    public const int MaxDeviceIdLength = 256;
}

public class UserSessionConfig : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.TokenHash)
            .HasMaxLength(Constraints.MaxHashLength)
            .IsRequired();
        
        builder.HasIndex(e => e.TokenHash)
            .IsUnique();

        builder.Property(e => e.DeviceId)
            .HasMaxLength(Constraints.MaxDeviceIdLength);
        
        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => new{ e.DeviceId, e.UserId })
            .IsUnique();
    }
}
