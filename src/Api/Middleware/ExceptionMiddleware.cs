using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Middleware;

public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteProblemDetails(ctx, ex);
        }
    }

    private static Task WriteProblemDetails(HttpContext ctx, Exception ex)
    {
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problem = new ProblemDetails
        {
            Status = ctx.Response.StatusCode,
            Title = "Internal Server Error",
            Detail = ctx.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment()
                ? ex.ToString()
                : "An unexpected error occurred.",
            Instance = ctx.Request.Path
        };

        return ctx.Response.WriteAsJsonAsync(problem);
    }
}