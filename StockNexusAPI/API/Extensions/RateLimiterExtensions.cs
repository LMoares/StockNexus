using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;


namespace StockNexusAPI.API.Extensions
{
    public static class RateLimiterExtensions
    {
        public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.ContentType = "application/problem+json";

                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        Status = StatusCodes.Status429TooManyRequests,
                        Title = "Too Many Requests",
                        Detail = "API rate limit exceeded. Please check your request frequency or back off temporarily.",
                        Instance = context.HttpContext.Request.Path
                    }, cancellationToken);
                };

                // POLICY A: Standard Partitioned API Limiter
                options.AddPolicy("StandardApiPolicy", httpContext =>
                {
                    string partitionKey = httpContext.User.Identity?.IsAuthenticated == true
                        ? httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "AuthUser"
                        : httpContext.Connection.RemoteIpAddress?.ToString() ?? "AnonymousIP";

                    return RateLimitPartition.GetTokenBucketLimiter(partitionKey, _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 60,
                        QueueLimit = 0,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(60),
                        TokensPerPeriod = 30,
                        AutoReplenishment = true
                    });
                });

                // POLICY B: Strict Auth Policy
                options.AddPolicy("StrictAuthPolicy", httpContext =>
                {
                    string ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "AnonymousIP";

                    return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    });
                });
            });

            return services;
        }
    }
}
