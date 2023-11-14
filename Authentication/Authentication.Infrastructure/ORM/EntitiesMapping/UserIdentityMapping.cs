using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;
public sealed class UserIdentityMapping : BaseMapping, IEntityTypeConfiguration<UserIdentity>
{
    public UserIdentityMapping()
        : base("Auth")
    {
        
    }

    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        builder.ToTable(nameof(UserIdentity), Schema);
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id_accountIdentity");

        builder.Property(c => c.UserStatus).HasColumnName("user_status").IsRequired();

        builder.Property(c => c.UserType).HasColumnType("tinyint")
               .HasColumnName("user_type").IsRequired();

        builder.Property(a => a.UserName).HasColumnName("login");

        builder.Property(a => a.PasswordHash).HasColumnName("password");

        builder.Property(a => a.Email).HasColumnName("email");

        builder.Property(a => a.EmailConfirmed).HasColumnName("email_confirmed");

        builder.Property(a => a.PhoneNumber).HasColumnName("cell_phone");

        builder.Property(a => a.PhoneNumberConfirmed).HasColumnName("cell_phone_confirmed");

        builder.Property(a => a.AccessFailedCount).HasColumnName("access_failed_count");

        builder.Property(a => a.NormalizedEmail).HasColumnName("normalized_email");

        builder.Property(a => a.NormalizedUserName).HasColumnName("normalized_login");

        builder.Property(a => a.LockoutEnabled).HasColumnName("lockout_enabled");

        builder.Property(a => a.ConcurrencyStamp).HasColumnName("concurrency_stamp");

        builder.Property(a => a.SecurityStamp).HasColumnName("security_stamp");

        builder.Property(a => a.TwoFactorEnabled).HasColumnName("two_factor_enabled");
    }
}
