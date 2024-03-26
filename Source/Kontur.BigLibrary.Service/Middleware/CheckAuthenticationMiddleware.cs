using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Kontur.BigLibrary.Service.Middleware
{
    public class CheckAuthenticationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity!.IsAuthenticated || 
                context.Request.Path.StartsWithSegments("/Identity"))
                await next(context);
            else
                await next(context);
        }
    }
}