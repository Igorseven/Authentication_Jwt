using Authentication.Domain.Entities;
using Authentication.Infrastructure.ORM.EntitiesMapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.ORM.EntitiesMapping;
public sealed class RefreshTokenMapping : BaseMapping, IEntityTypeConfiguration<RefreshToken>
{
    public RefreshTokenMapping()
        : base("Auth")
    {
        
    }

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(RefreshToken), Schema);
        builder.HasKey(r => r.RefreshTokenId);

        builder.Property(r => r.RefreshTokenId).HasColumnName("id_refreshToken");

        builder.Property(r => r.UserName).HasColumnType("varchar(150)").IsUnicode()
               .HasColumnName("user_name").IsRequired();

        builder.Property(r => r.Token).HasColumnType("nvarchar(Max)").IsUnicode()
               .HasColumnName("token").IsRequired();
    }
}
