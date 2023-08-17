using System.Security.Claims;

namespace Authentication.API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetUserName(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.Name)?.Value!;
    public static Guid GetUserIdentityId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        return Guid.Parse(userId);
    } 
}
