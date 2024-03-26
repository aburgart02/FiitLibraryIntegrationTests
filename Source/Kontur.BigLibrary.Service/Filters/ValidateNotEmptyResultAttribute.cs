using System.Net;
using Kontur.BigLibrary.Service.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kontur.BigLibrary.Service.Filters
{
    public class ValidateNotEmptyResultAttribute: ActionFilterAttribute
    {
        private readonly ErrorResponse notFoundResponse = new ErrorResponse()
        {
            Code = (int) HttpStatusCode.NotFound,
            Message = "Сущность не найдена."
        };

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value == null)
            {
                context.Result = new ObjectResult(notFoundResponse) {StatusCode = notFoundResponse.Code};
            }

            base.OnActionExecuted(context);
        }
    }
}