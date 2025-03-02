using JWT_Authentication.DTO;
using JWT_Authentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(TokenService tokenService, UserService userService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginUser)
        {
            var user = _userService.ValidateUser(loginUser.Username, loginUser.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { AccessToken = token, AccessTokenExpiryMinutes = _configuration["JwtSettings:AccessTokenExpirationMinutes"] });
        }
    }
}
