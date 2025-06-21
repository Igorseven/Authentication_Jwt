using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;

public sealed class RoleMapping : BaseMapping, IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role), Schema);
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnType("uniqueidentifier")
            .HasColumnName("id")
            .HasColumnOrder(1)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsUnicode()
            .HasColumnName("name")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(r => r.Active)
            .HasColumnType("bit")
            .HasColumnName("active")
            .HasColumnOrder(3)
            .IsRequired();

        builder.HasIndex(r => r.NormalizedName).IsUnique(false);
        builder.Property(r => r.NormalizedName)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsUnicode()
            .HasColumnName("normalized_name")
            .HasColumnOrder(4)
            .IsRequired(false);

        builder.Property(r => r.ConcurrencyStamp)
            .HasColumnName("concurrency_stamp")
            .HasColumnOrder(5)
            .IsRequired(false);
        
        builder.Property(r => r.Type)
            .HasColumnType("tinyint")
            .HasColumnName("type")
            .HasColumnOrder(6)
            .HasDefaultValue(ERoleType.Role)
            .IsRequired();

        builder.HasMany(r => r.RoleClaims)
            .WithOne()
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}