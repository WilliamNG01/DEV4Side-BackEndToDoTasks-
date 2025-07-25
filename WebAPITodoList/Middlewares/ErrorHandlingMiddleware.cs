using System.Net;
using System.Text.Json;

namespace ToDoListApi.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Prosegui nella pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore non gestito"); // Logga l'errore

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Si è verificato un errore imprevisto.",
#if DEBUG
                Details = ex.Message
#endif
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
