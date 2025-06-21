using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;

public sealed class UserTokenMapping : BaseMapping, IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(nameof(UserToken), Schema);
        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        builder.Property(ut => ut.UserId)
            .HasColumnType("uniqueidentifier")
            .HasColumnName("user_id")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(ut => ut.LoginProvider)
            .HasColumnType("nvarchar(450)")
            .HasColumnName("login_provider")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(ut => ut.Name)
            .HasColumnType("nvarchar(450)")
            .HasColumnName("name")
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property(ut => ut.Value)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("value")
            .HasColumnOrder(4)
            .IsRequired(false);
    }
}