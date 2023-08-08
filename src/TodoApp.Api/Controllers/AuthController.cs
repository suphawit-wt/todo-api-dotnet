using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            TokenResponse token = await _authService.Login(req);
            var res = new ApiResponse<TokenResponse>(data: token);

            return Ok(res);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            await _authService.Register(req);
            var res = new ApiResponse<object>(message: "Register Successfully!");

            return StatusCode(StatusCodes.Status201Created, res);
        }

    }
}