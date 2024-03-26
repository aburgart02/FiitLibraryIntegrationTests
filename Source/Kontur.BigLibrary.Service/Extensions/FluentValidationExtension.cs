using System.Linq;
using System.Net;
using Kontur.BigLibrary.Service.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Kontur.BigLibrary.Service.Extensions
{
    public static class FluentValidationExtension
    {
        public static void AddFluentValidationBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage))
                                        .ToArray();

                    var result = new ValidationResponse
                    {
                        Code = (int) HttpStatusCode.BadRequest,
                        Errors = errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });
        }
    }
}