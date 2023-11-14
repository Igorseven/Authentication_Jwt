namespace Authentication.API.Extensions;

public static class ExternalMethodExtension
{
    private const string MethodGet = "GET";
    public static bool IsMethodGet(dynamic context) => context.HttpContext.Request.Method == MethodGet;
}
