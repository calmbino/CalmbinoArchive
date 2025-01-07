using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CalmbinoArchive.Infrastructure.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger, IHostEnvironment environment)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        // API에서 HTTP Error(4xx, 5xx) throw를 사용할 경우 아래 코드 활성화
        // (int statusCode, string errorMsg) = exception switch
        // {
        //     //=> (403, null),
        //     ArgumentException argumentException => (400, argumentException.Message),
        //     BadHttpRequestException badrequestException => (400, badrequestException.Message),
        //     _ => (500, "Internal server error.")
        // };

        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType()
                            .Name,
            Title = exception.Message,
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = exception.Message
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails,
            cancellationToken: cancellationToken);

        return true;
    }
}