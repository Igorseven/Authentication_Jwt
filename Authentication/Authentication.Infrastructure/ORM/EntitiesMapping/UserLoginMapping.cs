using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;

public sealed class UserLoginMapping : BaseMapping, IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(nameof(UserLogin), Schema);
        builder.HasKey(ul => new { ul.ProviderKey, ul.LoginProvider });

        builder.Property(ul => ul.ProviderKey)
            .HasColumnType("nvarchar(450)")
            .HasColumnName("provider_key")
            .ValueGeneratedOnAdd()
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(ul => ul.LoginProvider)
            .HasColumnType("nvarchar(450)")
            .HasColumnName("login_provider")
            .ValueGeneratedOnAdd()
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(ul => ul.UserId)
            .HasColumnType("uniqueidentifier")
            .HasColumnName("user_id")
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property(ul => ul.ProviderDisplayName)
            .HasColumnType("nvarchar(450)")
            .HasColumnName("provider_display_name")
            .HasColumnOrder(4)
            .IsRequired(false);

    }
}