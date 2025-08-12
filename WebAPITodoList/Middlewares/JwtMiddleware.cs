using System.Net;
using System.Text.Json;
using WebAPITodoList.Services;

namespace ToDoListApi.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var principal = tokenService.ValidateToken(token);

            if (principal != null)
            {
                context.User = principal; // set the user principal for the request
            }
        }

        await _next(context);
    }
}
