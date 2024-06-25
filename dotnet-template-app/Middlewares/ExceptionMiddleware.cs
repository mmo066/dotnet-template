using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dotnet-template-app.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        

        var errorId = Guid.NewGuid().ToString();
        _logger.LogError($"{errorId}: {context.Request.Path} - {exception.Source} - {exception.Message}: {exception.StackTrace}");

        var response = _env.IsDevelopment()
            ? new ProblemDetails
            {
                Title = exception.Message,
                Status = context.Response.StatusCode,
                Detail = errorId + ": " + exception.StackTrace,
            }
            : new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = context.Response.StatusCode,
                Detail = errorId
            };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}