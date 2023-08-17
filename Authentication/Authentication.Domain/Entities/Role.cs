using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;
public sealed class Role : IdentityRole<Guid>
{
    public List<UserRole>? UserRoles { get; set; }
}
