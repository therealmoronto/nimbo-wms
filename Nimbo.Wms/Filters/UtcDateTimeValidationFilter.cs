using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nimbo.Wms.Filters;

public class UtcDateTimeValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is null) continue;

            var errors = new List<string>();
            ValidateObject(arg, "$", errors);

            if (errors.Count > 0)
            {
                context.Result = new BadRequestObjectResult(new ProblemDetails
                {
                    Title = "Invalid timestamp",
                    Detail = "All DateTime values must be UTC (ISO-8601 with 'Z').",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = errors }
                });
                return;
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
    
    private static void ValidateObject(object obj, string path, List<string> errors)
    {
        if (obj is DateTime dt)
        {
            if (dt.Kind is DateTimeKind.Unspecified)
                errors.Add($"{path}: DateTime.Kind must be Utc, but was {dt.Kind}.");

            return;
        }

        if (obj is DateTimeOffset)
        {
            // DateTimeOffset is always unambiguous; allow.
            return;
        }

        var type = obj.GetType();
        if (type.IsPrimitive || obj is string || obj is Guid || obj is decimal)
            return;

        if (obj is IEnumerable enumerable)
        {
            var i = 0;
            foreach (var item in enumerable)
            {
                if (item is null)
                {
                    i++;
                    continue;
                }

                ValidateObject(item, $"{path}[{i}]", errors);
                i++;
            }
            return;
        }

        foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
        {
            if (!prop.CanRead) continue;

            var value = prop.GetValue(obj);
            if (value is null) continue;

            ValidateObject(value, $"{path}.{prop.Name}", errors);
        }
    }
}
