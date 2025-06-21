using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;

public sealed class UserClaimMapping : BaseMapping, IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable(nameof(UserClaim), Schema);
        builder.HasKey(uc => uc.Id);

        builder.Property(uc => uc.Id)
            .HasColumnType("int")
            .HasColumnName("id")
            .HasColumnOrder(1);

        builder.Property(uc => uc.UserId)
            .HasColumnType("uniqueidentifier")
            .HasColumnName("user_id")
            .HasColumnOrder(4)
            .IsRequired();

        builder.Property(uc => uc.ClaimType)
            .HasColumnType("varchar(256)")
            .HasColumnName("claim_type")
            .HasColumnOrder(2)
            .IsRequired(false);

        builder.Property(uc => uc.ClaimValue)
            .HasColumnType("varchar(256)")
            .HasColumnName("claim_value")
            .HasColumnOrder(3)
            .IsRequired(false);
    }
}