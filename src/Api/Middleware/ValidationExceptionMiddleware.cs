using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Api.Middleware;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationExceptionMiddleware> _logger;

    public ValidationExceptionMiddleware(
        RequestDelegate next,
        ILogger<ValidationExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            await ManejarValidacionAsync(context, exception);
        }
        catch (InvalidOperationException exception)
        {
            await ManejarConflictoAsync(context, exception);
        }
        catch (Exception exception)
        {
            await ManejarErrorInesperadoAsync(context, exception);
        }
    }

    private async Task ManejarValidacionAsync(
        HttpContext context,
        ValidationException exception)
    {
        _logger.LogWarning(
            "Error de validación en la solicitud: {Errores}",
            exception.Errors.Select(x => x.ErrorMessage));

        var errores = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo
                    .Select(error => error.ErrorMessage)
                    .ToArray());

        var respuesta = new ValidationProblemDetails(errores)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Error de validación"
        };

        context.Response.StatusCode =
            StatusCodes.Status400BadRequest;

        await context.Response.WriteAsJsonAsync(respuesta);
    }

    private async Task ManejarConflictoAsync(
        HttpContext context,
        InvalidOperationException exception)
    {
        _logger.LogWarning(
            exception,
            "Se produjo un conflicto durante la solicitud.");

        var respuesta = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflicto",
            Detail = exception.Message
        };

        context.Response.StatusCode =
            StatusCodes.Status409Conflict;

        await context.Response.WriteAsJsonAsync(respuesta);
    }

    private async Task ManejarErrorInesperadoAsync(
        HttpContext context,
        Exception exception)
    {
        _logger.LogError(
            exception,
            "Ocurrió un error inesperado procesando la solicitud.");

        var respuesta = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error interno del servidor",
            Detail = "Ocurrió un error inesperado."
        };

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(respuesta);
    }
}