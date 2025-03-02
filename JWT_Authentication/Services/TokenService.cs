using JWT_Authentication.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Authentication.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? string.Empty);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("RandomId", Guid.NewGuid().ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.Username),
                new Claim("Name", user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Issuer = _configuration["JwtSettings:Issuer"] ?? string.Empty,
            Audience = _configuration["JwtSettings:Audience"] ?? string.Empty,
            Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:AccessTokenExpirationMinutes"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task ValidateToken(HttpContext context, string token)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? string.Empty);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidIssuer = _configuration["JwtSettings:Issuer"] ?? string.Empty,
                ValidateIssuer = true, // Enable issuer validation

                ValidAudience = _configuration["JwtSettings:Audience"] ?? string.Empty,
                ValidateAudience = true, // Enable audience validation

                ValidateLifetime = true, // Ensure token expiration is checked
                ClockSkew = TimeSpan.Zero  // Remove default 5-minute clock skew
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            context.Items["User"] = principal.Identity?.Name; // Store user identity for later use
        }
        catch
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid Token.");
            return;
        }
    }
}
