using AuthService.Domain.Entities;
using AuthService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.EntityConfiguration;

file static class Constraints
{
    public const int MailMaxLength = 256;
}

public sealed class AuthenticationCredentialsConfig : IEntityTypeConfiguration<AuthenticationCredentials>
{
    public void Configure(EntityTypeBuilder<AuthenticationCredentials> builder)
    {
        builder.HasKey(e => e.UserId);

        builder.HasIndex(e => e.UserId);
        
        builder.Property(e => e.Mail)
            .HasConversion(
                mail => mail.Value,
                value => Email.FromDatabase(value))
            .IsRequired()
            .HasMaxLength(Constraints.MailMaxLength);
        
        builder.HasIndex(e => e.Mail)
            .IsUnique();
        
        builder.Property(e => e.Password)
            .HasConversion(
                password => password.Value,
                value => HashedString.FromDatabase(value))
            .IsRequired();;
    }
}
