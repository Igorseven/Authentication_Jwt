namespace Authentication.Domain.Extensions;
public static class InterpolationExtension
{
    public static string FormatTo(this string message, params object[] args)
    {
        return string.Format(message, args);
    }
}
