using System.Net;
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

        var problemDetails = GetProblemDetails(exception);
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType(), cancellationToken: cancellationToken);
        return true;
    }

    private static ProblemDetails GetProblemDetails(Exception exception)
        => exception switch
        {
            BadRequestException ex => new HttpValidationProblemDetails(ex.Errors ?? [])
            {
                Status = (int)ex.StatusCode,
                Title = ex.StatusCode.ToString(),
                Detail = ex.Message
            },
            BoardException ex => new ProblemDetails
            {
                Status = (int)ex.StatusCode,
                Title = ex.StatusCode.ToString(),
                Detail = ex.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = nameof(HttpStatusCode.InternalServerError),
                Detail = InternalServerErrorMessage
            }
        };
}
