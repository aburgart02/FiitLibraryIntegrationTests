using Kontur.BigLibrary.Service.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Kontur.BigLibrary.Service.Extensions
{
    public static class CustomExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}