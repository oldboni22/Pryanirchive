using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data.EntityConfiguration;

public sealed class UserScapePermissionEntityConfig : IEntityTypeConfiguration<UserSpacePermission>
{
    public void Configure(EntityTypeBuilder<UserSpacePermission> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new {e.UserId, e.SpaceId})
            .IsUnique();
    }
}
