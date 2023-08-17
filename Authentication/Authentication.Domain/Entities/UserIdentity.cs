using Authentication.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;
public sealed class UserIdentity : IdentityUser<Guid>
{
    public EUserType UserType { get; set; }
    public EUserStatus UserStatus { get; set; }
    public List<UserRole>? UserRoles { get; set; }
}
