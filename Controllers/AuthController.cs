using Microsoft.AspNetCore.Mvc;
using todo_api_sqlserver.Models;
using todo_api_sqlserver.Models.Requests;
using todo_api_sqlserver.Models.Responses;
using todo_api_sqlserver.Services.Auth;

namespace todo_api_sqlserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            var responseMessage = new MessageResponse { };

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("400");
                }

                var accessToken = await _authService.Login(req);

                var response = new LoginResponse
                {
                    AccessToken = accessToken
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "400":
                        responseMessage.Message = "Bad Request";

                        return BadRequest(responseMessage);
                    case "401":
                        responseMessage.Message = "Username or Password is invalid";

                        return Unauthorized(responseMessage);
                    default:
                        responseMessage.Message = "Internal Server Error";

                        return StatusCode(500, responseMessage);
                }
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] User req)
        {
            var responseMessage = new MessageResponse { };

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("400");
                }

                await _authService.Register(req);

                responseMessage.Message = "Register Successfully!";

                return StatusCode(201, responseMessage);
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "400":
                        responseMessage.Message = "Bad Request";

                        return BadRequest(responseMessage);
                    case "409":
                        responseMessage.Message = "This Username already taken!";

                        return Conflict(responseMessage);
                    default:
                        responseMessage.Message = "Internal Server Error";

                        return StatusCode(500, responseMessage);
                }
            }
        }

    }
}
