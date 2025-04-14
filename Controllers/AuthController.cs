using JobPortalAPI.DTOs;
using JobPortalAPI.Models;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser)
        {
            var user = new User
            {
                Email = registerUser.Email,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Role = registerUser.Role
            };

            var result = await _authService.RegisterUserAsync(user, registerUser.Password);
            if (!result)
                return BadRequest("Registration failed. Email might already be in use.");

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var token = await _authService.LoginUserAsync(login.Email, login.PasswordHash);
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }
    }
}
