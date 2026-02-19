using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Data.EntityConfiguration;

public sealed class UserEntityConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Permissions)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.Name)
            .HasConversion(
                v => v.ToString(),
                value => UserName.FromDatabase(value))
            .IsRequired()
            .HasMaxLength(UserName.MaxNameLength);
        
        builder.Property(e => e.Tag)
            .HasConversion(
                v => v.ToString(),
                value => UserTag.FromDatabase(value))
            .HasMaxLength(UserTag.Size)
            .IsFixedLength()
            .IsRequired();

        builder.Property(e => e.AvatarId)
            .HasConversion(
                v => v == null ? null : v.ToString(),
                value => UserAvatarId.FromDatabase(value))
            .HasMaxLength(UserAvatarId.MaxFieldLength)
            .IsFixedLength()
            .IsRequired(false);
        
        builder.HasIndex(e => e.Tag)
            .IsUnique();
    }
}
