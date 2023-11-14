using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;
public sealed class UserRole : IdentityUserRole<Guid>
{
    public UserIdentity? User { get; set; }
    public Role? Role { get; set; }
}
