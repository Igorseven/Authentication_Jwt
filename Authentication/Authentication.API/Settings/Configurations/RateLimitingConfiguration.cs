using System.Threading.RateLimiting;
using Authentication.API.Settings.Constants;

namespace Authentication.API.Settings.Configurations;

public static class RateLimitingConfiguration
{
    private const int ToManyRequests = StatusCodes.Status429TooManyRequests;

    public static void AddRateLimitingConfiguration(this IServiceCollection services)
    {
        services.AddRateLimiter(config =>
        {
            config.AddPolicy(RateLimitName.LimitingByIp, httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromSeconds(3)
                    })).RejectionStatusCode = ToManyRequests;
        });

        services.AddRateLimiter(config =>
        {
            config.AddPolicy(RateLimitName.LimitingByUser, httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(2)
                    })).RejectionStatusCode = ToManyRequests;
        });
    }
}