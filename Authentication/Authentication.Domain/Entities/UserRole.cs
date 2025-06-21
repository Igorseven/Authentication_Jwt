using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;
public sealed class UserRole : IdentityUserRole<Guid>
{
    public User? User { get; set; }
    public Role? Role { get; set; }
}
