namespace Authentication.API.Extensions;

public static class ExternalMethodExtension
{
    private const string METHOD_GET = "GET";
    public static bool IsMethodGet(dynamic context) => context.HttpContext.Request.Method == METHOD_GET;
}
