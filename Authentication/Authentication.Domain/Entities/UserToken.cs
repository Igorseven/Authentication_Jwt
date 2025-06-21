using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;

public sealed class UserToken : IdentityUserToken<Guid>
{

}