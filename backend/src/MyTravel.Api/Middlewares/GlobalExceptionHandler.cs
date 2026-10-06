using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyTravel.Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Excepción no controlada capturada: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado"),
            BadHttpRequestException => (StatusCodes.Status400BadRequest, "Petición incorrecta"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "Ha ocurrido un error inesperado al procesar la solicitud."
                : exception.Message,
            Type = "https://tools.ietf.org/html/rfc7807",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
