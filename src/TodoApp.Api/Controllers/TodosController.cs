using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;

namespace TodoApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ITokenService _tokenService;

        public TodosController(ITodoService todoService, ITokenService tokenService)
        {
            _todoService = todoService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            int userId = _tokenService.GetUserId(User);
            IEnumerable<TodoResponse> todos = await _todoService.GetTodos(userId);
            var res = new ApiResponse<IEnumerable<TodoResponse>>(todos);

            return Ok(res);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            int userId = _tokenService.GetUserId(User);
            TodoResponse todo = await _todoService.GetTodo(id, userId);
            var res = new ApiResponse<TodoResponse>(todo);

            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTodo([FromBody] TodoRequest req)
        {
            int userId = _tokenService.GetUserId(User);
            TodoResponse todo = await _todoService.CreateTodo(req, userId);
            var res = new ApiResponse<TodoResponse>(todo);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, res);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoRequest req)
        {
            int userId = _tokenService.GetUserId(User);
            TodoResponse todo = await _todoService.UpdateTodo(id, req, userId);
            var res = new ApiResponse<TodoResponse>(todo);

            return Ok(res);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            int userId = _tokenService.GetUserId(User);
            await _todoService.DeleteTodo(id, userId);

            return NoContent();
        }

    }
}