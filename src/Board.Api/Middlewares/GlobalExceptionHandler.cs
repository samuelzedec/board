using Board.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Middlewares;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string InternalServerErrorMessage
        = "Ocorreu um erro inesperado. Tente novamente mais tarde.";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {ExceptionType} - {Message}",
            exception.GetType().Name, exception.Message);

        var problemDetails = GetProblemDetails(httpContext, exception);
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static ProblemDetails GetProblemDetails(HttpContext httpContext, Exception exception)
        => exception switch
        {
            BadRequestException ex => new HttpValidationProblemDetails(ex.Errors ?? [])
            {
                Status = (int)ex.StatusCode, Title = ex.Message, Instance = httpContext.Request.Path
            },
            BoardException ex => new ProblemDetails
            {
                Status = (int)ex.StatusCode, Title = ex.Message, Instance = httpContext.Request.Path
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = InternalServerErrorMessage,
                Instance = httpContext.Request.Path
            }
        };
}