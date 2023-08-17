using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;
public sealed class RoleMapping : BaseMapping, IEntityTypeConfiguration<Role>
{
    public RoleMapping()
        : base("Auth")
    {
        
    }

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role), Schema);
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("id_role");

        builder.Property(r => r.Name).HasColumnType("varchar(250)").IsUnicode(true)
               .HasColumnName("name").IsRequired(true);

        builder.Property(r => r.NormalizedName).HasColumnType("varchar(250)").IsUnicode(true)
               .HasColumnName("normalized_name");

        builder.Property(r => r.ConcurrencyStamp).HasColumnName("concurrency_stamp");
    }
}
