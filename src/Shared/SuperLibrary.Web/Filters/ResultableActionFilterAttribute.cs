using SuperLibrary.Web.Models;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.Results.Generic.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IResult = SuperLibrary.Web.Results.Abstract.IResult;

namespace SuperLibrary.Web.Filters;

public class ResultableActionFilterAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not ResultableActionResult resultableActionResult)
            return;

        if (resultableActionResult.Result.IsSuccess)
        {
            if (resultableActionResult.Result is IResult<object> objectResult)
            {
                context.Result = new OkObjectResult(objectResult.Data);
                return;
            }

            context.Result = new OkResult();

            return;
        }

        context.Result = resultableActionResult.Result.ResultStatus switch
        {
            ResultStatus.InvalidArgument => new BadRequestObjectResult(CreateError(resultableActionResult.Result)),
            ResultStatus.Forbidden => new ForbidResult(new AuthenticationProperties(resultableActionResult.Result.Errors)),
            ResultStatus.Unauthenticated => new UnauthorizedObjectResult(CreateError(resultableActionResult.Result)),
            ResultStatus.NotFound => new ObjectResult(CreateError(resultableActionResult.Result))
            {
                StatusCode = StatusCodes.Status404NotFound
            },
            ResultStatus.InternalError => new StatusCodeResult(StatusCodes.Status500InternalServerError),
            _ => new BadRequestObjectResult(CreateError(resultableActionResult.Result))
        };
    }

    private static ErrorResponse CreateError(IResult result)
        => new(result.Errors, result.ResultStatus);
}
