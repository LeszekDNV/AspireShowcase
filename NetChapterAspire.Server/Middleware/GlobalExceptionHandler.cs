using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using NetChapterAspire.Server.Models.Common;

namespace NetChapterAspire.Server.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var response = ApiResponse.ErrorResponse(
            "An error occurred while processing your request.",
            new Dictionary<string, string>
            {
                ["exceptionType"] = exception.GetType().Name,
                ["details"] = httpContext.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment() 
                    ? exception.Message 
                    : "Internal server error"
            }
        );

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
