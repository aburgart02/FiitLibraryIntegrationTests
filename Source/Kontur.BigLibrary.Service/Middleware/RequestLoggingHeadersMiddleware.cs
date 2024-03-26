using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vostok.Logging.Abstractions;

namespace Kontur.BigLibrary.Service.Middleware
{
    public class RequestLoggingHeadersMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILog logger;

        public RequestLoggingHeadersMiddleware(RequestDelegate next, ILog logger)
        {
            this.next = next;
            this.logger = logger;
        }
        
        public async Task Invoke(HttpContext context)
        {
            
            var builder = new StringBuilder(Environment.NewLine);
            foreach (var header in context.Request.Headers)
            {
                builder.AppendLine($"{header.Key}:{header.Value}");
            }
            logger.Info(builder.ToString());
            await next.Invoke(context);
        }
    }
}