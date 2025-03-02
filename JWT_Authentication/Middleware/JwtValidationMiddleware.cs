using JWT_Authentication.Services;

namespace JWT_Authentication.Middleware;

public class JwtValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenService _tokenService;

    public JwtValidationMiddleware(RequestDelegate next, TokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            // Proceed without authentication if no token is provided
            await _next(context);
            return;
        }

        await _tokenService.ValidateToken(context, token);

        await _next(context);
    }
}
