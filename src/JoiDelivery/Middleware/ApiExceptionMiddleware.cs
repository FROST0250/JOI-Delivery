using JoiDelivery.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JoiDelivery.Middleware;

public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApplicationValidationException exception)
        {
            await WriteProblemDetails(context, StatusCodes.Status400BadRequest, "Validation failed", exception.Message);
        }
        catch (ApplicationNotFoundException exception)
        {
            await WriteProblemDetails(context, StatusCodes.Status404NotFound, "Resource not found", exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception.");
            await WriteProblemDetails(
                context,
                StatusCodes.Status500InternalServerError,
                "Unexpected error",
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemDetails(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
