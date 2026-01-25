using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data.EntityConfiguration;

public class UserGroupPermissionEntityConfig : IEntityTypeConfiguration<UserGroupPermission>
{
    public void Configure(EntityTypeBuilder<UserGroupPermission> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new {e.UserId, e.FileGroupId})
            .IsUnique();
    }
}
