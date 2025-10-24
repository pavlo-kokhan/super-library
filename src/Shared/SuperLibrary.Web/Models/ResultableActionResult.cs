using Microsoft.AspNetCore.Mvc;
using SuperLibrary.Web.Results.Abstract;

namespace SuperLibrary.Web.Models;

public class ResultableActionResult : ActionResult
{
    public ResultableActionResult(IResult result)
    {
        Result = result;
    }

    public IResult Result { get; }
}
