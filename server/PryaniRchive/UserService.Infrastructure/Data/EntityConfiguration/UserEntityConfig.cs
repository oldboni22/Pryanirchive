using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int UserNameMaxLength = 15;

    public const int TagLength = 8;
}

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
            .HasMaxLength(Constraints.UserNameMaxLength);
        
        builder.Property(e => e.Tag)
            .HasConversion(
                v => v.ToString(),
                value => UserTag.FromDatabase(value))
            .HasMaxLength(Constraints.TagLength)
            .IsFixedLength()
            .IsRequired();

        builder.Property(e => e.AvatarId)
            .HasConversion(
                v => v == null ? null : v.ToString(),
                value => value == null ? null : UserAvatarId.FromDatabase(value))
            .HasMaxLength(UserAvatarId.MaxFieldLength)
            .IsFixedLength()
            .IsRequired(false);
        
        builder.HasIndex(e => e.Tag)
            .IsUnique();
    }
}
