using Authentication.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;
public sealed class User : IdentityUser<Guid>
{
    public EUserType Type { get; set; }
    public EUserStatus Status { get; set; }
    public DateTime CreationDate { get; init; }
    
    public List<UserRole>? UserRoles { get; set; }
    public List<UserToken>? UserTokens { get; set; }
    public List<UserClaim>? UserClaims { get; set; }
}
