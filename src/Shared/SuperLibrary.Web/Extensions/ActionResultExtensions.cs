using Microsoft.AspNetCore.Mvc;
using SuperLibrary.Web.Models;
using SuperLibrary.Web.Results.Abstract;

namespace SuperLibrary.Web.Extensions;

public static class ActionResultExtensions
{
    public static IActionResult ToActionResult(this IResult result)
        => new ResultableActionResult(result);
}
