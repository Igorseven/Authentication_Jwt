using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;

public sealed class UserMapping : BaseMapping, IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), Schema);
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnType("uniqueidentifier")
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasColumnOrder(1)
            .IsRequired();
        
        builder.Property(u => u.Status)
            .HasColumnType("tinyint")
            .HasColumnName("status")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(u => u.UserName)
            .HasColumnType("nvarchar(256)")
            .HasColumnOrder(3)
            .HasColumnName("login");

        builder.Property(u => u.PasswordHash)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("password")
            .HasColumnOrder(4);

        builder.Property(u => u.Email)
            .HasColumnType("nvarchar(256)")
            .HasColumnName("email")
            .HasColumnOrder(5);

        builder.Property(u => u.EmailConfirmed)
            .HasColumnType("bit")
            .HasColumnName("email_confirmed");

        builder.Property(u => u.PhoneNumber)
            .HasColumnType("varchar(20)")
            .HasColumnName("cell_phone");

        builder.Property(u => u.PhoneNumberConfirmed)
            .HasColumnType("bit")
            .HasColumnName("cell_phone_confirmed");

        builder.Property(u => u.AccessFailedCount)
            .HasColumnName("access_failed_count");

        builder.Property(u => u.NormalizedEmail)
            .HasColumnType("nvarchar(256)")
            .HasColumnName("normalized_email");
        
        builder.Property(u => u.NormalizedUserName)
            .HasColumnType("nvarchar(256)")
            .HasColumnName("normalized_login");

        builder.Property(u => u.LockoutEnabled)
            .HasColumnType("bit")
            .HasColumnName("lockout_enabled");

        builder.Property(u => u.ConcurrencyStamp)
            .HasColumnName("concurrency_stamp");

        builder.Property(u => u.SecurityStamp)
            .HasColumnName("security_stamp");

        builder.Property(u => u.TwoFactorEnabled)
            .HasColumnType("bit")
            .HasColumnName("two_factor_enabled");

        builder.Property(u => u.LockoutEnd)
            .HasColumnName("lockout_end");
        
         builder.Property(u => u.CreationDate)
             .HasColumnType("datetime2")
            .HasColumnName("creation_date")
             .IsRequired();

        builder.HasMany(u => u.UserClaims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasMany(u => u.UserTokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}