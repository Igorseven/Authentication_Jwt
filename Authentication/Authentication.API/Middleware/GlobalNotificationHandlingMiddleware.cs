using Authentication.Domain.Handlers.NotificationHandler;

namespace Authentication.API.Middleware;

public sealed class GlobalNotificationHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalNotificationHandlingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(
                new DomainNotification("Error", "Server side error"),
                CancellationToken.None);
        }
    }
}