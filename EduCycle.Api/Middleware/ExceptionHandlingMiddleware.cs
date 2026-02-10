using EduCycle.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace EduCycle.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(
                context,
                (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred"
            );
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            success = false,
            message,
            errors = Array.Empty<string>()
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }
}
