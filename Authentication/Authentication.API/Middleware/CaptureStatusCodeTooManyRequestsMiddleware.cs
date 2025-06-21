using Authentication.Domain.Handlers.NotificationHandler;

namespace Authentication.API.Middleware;

public sealed class CaptureStatusCodeTooManyRequestsMiddleware
{
    private readonly RequestDelegate _next;
    
    public CaptureStatusCodeTooManyRequestsMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _next(httpContext);

        if (httpContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            await httpContext.Response.WriteAsJsonAsync(
                new DomainNotification("Error", "Too Many Requests Server"),
                CancellationToken.None);
    }
    
}