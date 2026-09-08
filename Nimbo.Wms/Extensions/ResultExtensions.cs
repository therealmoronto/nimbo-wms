using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts;

namespace Nimbo.Wms.Extensions;

[PublicAPI]
public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        return result.IsSuccess
            ? controller.NoContent()
            : controller.Problem(result.Error);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller, string actionName)
    {
        return result.IsSuccess
            ? controller.CreatedAtAction(actionName, new { result.Value })
            : controller.Problem(result.Error);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        return result.IsSuccess
            ? controller.Ok(new { result.Value })
            : controller.Problem(result.Error);
    }

    private static ObjectResult Problem(this ControllerBase controller, Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.BusinessRule => StatusCodes.Status422UnprocessableEntity,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => throw new ArgumentOutOfRangeException()
        };

        return controller.Problem(
            detail: error.Message,
            title: error.Code,
            statusCode: statusCode);
    }
}
