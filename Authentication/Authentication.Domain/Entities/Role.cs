using Authentication.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;
public sealed class Role : IdentityRole<Guid>
{
    public bool Active { get; set; }
    public ERoleType Type { get; init; }
    
    public List<UserRole>? UserRoles { get; set; }
    public List<RoleClaim>? RoleClaims { get; set; }
}
