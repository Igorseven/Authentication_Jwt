using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.ORM.Context;
public sealed class ApplicationContext : IdentityDbContext<UserIdentity,
                                                           Role,
                                                           Guid,
                                                           IdentityUserClaim<Guid>,
                                                           UserRole, IdentityUserLogin<Guid>,
                                                           IdentityRoleClaim<Guid>,
                                                           IdentityUserToken<Guid>>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> dbContext)
         : base(dbContext)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}
