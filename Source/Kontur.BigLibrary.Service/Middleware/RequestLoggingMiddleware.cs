using System;
using System.Net;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Vostok.Logging.Abstractions;

namespace Kontur.BigLibrary.Service.Middleware
{
   public class RequestLoggingMiddleware
   {
      private const string JsonContentType = "application/json";
      private readonly RequestDelegate next;
      private readonly ILog logger;

      public RequestLoggingMiddleware(RequestDelegate next, ILog logger)
      {
         this.next = next;
         this.logger = logger;
      }

      public async Task Invoke(HttpContext context)
      {
         try
         {
            LogRequest(context);
            await next.Invoke(context);
         }
         catch (Exception ex)
         {
            await HandleExceptionAsync(context, ex);
         }
      }

      private async Task HandleExceptionAsync(HttpContext context, Exception ex)
      {
         LogRequest(context, ex);

         var response = CreateResponse(ex);
         context.Response.StatusCode = response.Code;
         context.Response.ContentType = JsonContentType;

         await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
      }

      private void LogRequest(HttpContext context, Exception ex = null)
      {
         var method = context.Request.Method;
         var path = GetPath(context);
         var statusCode = context.Response.StatusCode;

         if (ex != null)
         {
            logger.Error(ex, "Ошибка при обработке запроса. Код ответа = {statusCode}. {method} {path} {statusCode}", statusCode, method, path);
         }
         else
         {
            logger.Info("Код ответа = {statusCode}. {method} {path} {statusCode}", statusCode, method, path);
         }
      }

      private ErrorResponse CreateResponse(Exception exception)
      {
         switch (exception)
         {
            case var _ when exception is EntityNotFoundException:
               return new ErrorResponse()
                  {
                     Code = (int) HttpStatusCode.NotFound,
                     Message = exception.Message
                  }
               ;
            case var _ when exception is ValidationException:
               return new ValidationResponse()
               {
                  Code = (int) HttpStatusCode.BadRequest,
                  Errors = new[] {exception.Message}
               };
            default:
               return new ErrorResponse()
               {
                  Code = (int) HttpStatusCode.InternalServerError,
                  Message = exception.Message
               };
         }
      }

      private static string GetPath(HttpContext httpContext)
      {
         return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? httpContext.Request.Path.ToString();
      }
   }
}