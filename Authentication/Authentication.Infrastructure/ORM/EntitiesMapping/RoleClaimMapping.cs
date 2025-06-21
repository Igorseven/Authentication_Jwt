using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;

public sealed class RoleClaimMapping : BaseMapping, IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable(nameof(RoleClaim), Schema);
        builder.HasKey(ur => ur.Id);

        builder.Property(ur => ur.Id)
            .HasColumnType("int")
            .HasColumnName("id")
            .HasColumnOrder(1);

        builder.Property(ur => ur.RoleId)
            .HasColumnType("uniqueidentifier")
            .HasColumnName("role_id")
            .HasColumnOrder(4)
            .IsRequired();

        builder.Property(ur => ur.ClaimType)
            .HasColumnType("varchar(256)")
            .HasColumnName("claim_type")
            .HasColumnOrder(2)
            .IsRequired(false);

        builder.Property(ur => ur.ClaimValue)
            .HasColumnType("varchar(256)")
            .HasColumnName("claim_value")
            .HasColumnOrder(3)
            .IsRequired(false);
    }
}