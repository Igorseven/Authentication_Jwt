using Authentication.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace Authentication.UnitTest.TestTools;
public sealed class UtilTools
{
    public static IFormFile BuildIFormFile(string extension = "pdf")
    {
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");

        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", $"image.{extension}")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg",
            ContentDisposition = $"form-data; name=\"Image\"; filename=\"image.{extension}\""
        };
    }


    public static ClaimsPrincipal BuildClaimPrincipal(string userName, EUserType userType)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                new (ClaimTypes.NameIdentifier, userName),
                new (ClaimTypes.Role, EUserType.Client.ToString())
        }));
    }

    public static Func<IQueryable<T>, IIncludableQueryable<T, object>> BuildQueryableIncludeFunc<T>() where T : class =>
           It.IsAny<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();

    public static Expression<Func<T, bool>> BuildPredicateFunc<T>() where T : class =>
           It.IsAny<Expression<Func<T, bool>>>();

    public static Expression<Func<T, T>> BuildSelectorFunc<T>() where T : class =>
        It.IsAny<Expression<Func<T, T>>>();
}
