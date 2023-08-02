using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_api_sqlserver.Models;
using todo_api_sqlserver.Models.Responses;
using todo_api_sqlserver.Services.Todos;

namespace todo_api_sqlserver.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetAll()
        {
            var responseMessage = new MessageResponse { };

            try
            {
                List<Todo> todos = await _todoService.GetAll();

                return Ok(todos);
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    default:
                        responseMessage.Message = "Internal Server Error";

                        return StatusCode(500, responseMessage);
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById(int id)
        {
            var responseMessage = new MessageResponse { };

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("400");
                }

                Todo? todo = await _todoService.GetById(id);

                return Ok(todo);
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "400":
                        responseMessage.Message = "Bad Request";

                        return BadRequest(responseMessage);
                    case "403":
                        responseMessage.Message = "Forbidden";

                        return StatusCode(403, responseMessage);
                    case "404":
                        responseMessage.Message = "Not Found";

                        return NotFound(responseMessage);
                    default:
                        responseMessage.Message = "Internal Server Error";

                        return StatusCode(500, responseMessage);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Todo req)
        {
            var responseMessage = new MessageResponse { };

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("400");
                }

                await _todoService.Create(req);

                responseMessage.Message = "Created Todo Successfully!";

                return StatusCode(201, responseMessage);
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "400":
                        responseMessage.Message = "Bad Request";

                        return BadRequest(responseMessage);
                    default:
                        responseMessage.Message = "Internal Server Error";

                        return StatusCode(500, responseMessage);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Todo req)
        {
            var responseMessage = new MessageResponse { };

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("400");
                }

                await _todoService.Update(id, req);

                responseMessage.Message = "Updated Todo Successfully!";

                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "400":
                        responseMessage.Message = "Bad Request";

                        return BadRequest(responseMessage);
                    case "403":
                        responseMessage.Message = "Forbidden";

                        return StatusCode(403, responseMessage);
                    case "404":
                        responseMessage.Message = "Not Found";

                        return NotFound(responseMessage);
                    default:
                        responseMessage.Message = "Internal Server Error";

                        return StatusCode(500, responseMessage);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var responseMessage = new MessageResponse { };

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("400");
                }

                await _todoService.Delete(id);

                responseMessage.Message = "Deleted Todo Successfully!";

                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "400":
                        responseMessage.Message = "Bad Request";

                        return BadRequest(responseMessage);
                    case "403":
                        responseMessage.Message = "Forbidden";

                        return StatusCode(403, responseMessage);
                    case "404":
                        responseMessage.Message = "Not Found";

                        return NotFound(responseMessage);
                    default:
                        responseMessage.Message = "Internal Server Error";

                        return StatusCode(500, responseMessage);
                }
            }
        }

    }
}
