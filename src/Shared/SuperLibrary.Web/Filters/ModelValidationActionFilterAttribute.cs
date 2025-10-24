using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuperLibrary.Web.Models;
using SuperLibrary.Web.Results;

namespace SuperLibrary.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ModelValidationActionFilterAttribute: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errorsDictionary = context
                .ModelState
                .ToDictionary(ms => ms.Key, ms => ms.Value?.Errors.FirstOrDefault()?.ErrorMessage);

            context.Result = new BadRequestObjectResult(new ErrorResponse(errorsDictionary, ResultStatus.InvalidArgument));
        }
    }
}
