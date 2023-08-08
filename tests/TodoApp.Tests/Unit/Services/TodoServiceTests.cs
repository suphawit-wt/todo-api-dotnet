using Xunit;
using Moq;
using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;
using TodoApp.Core.Services;

namespace TodoApp.Tests.Unit.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _mockTodoRepository;
        private readonly ITodoService _todoService;

        public TodoServiceTests()
        {
            _mockTodoRepository = new Mock<ITodoRepository>();
            _todoService = new TodoService(_mockTodoRepository.Object);
        }

        [Fact]
        public async Task GetTodos()
        {
            int userId = 1;
            IEnumerable<Todo> mock = new List<Todo> {
                new Todo { Id = 1, Title = "Test Todo 1", Completed = false, UserId = userId },
                new Todo { Id = 2, Title = "Test Todo 2", Completed = false, UserId = userId },
            };

            _mockTodoRepository.Setup(repo => repo.GetAllByUserId(userId)).ReturnsAsync(mock);

            IEnumerable<TodoResponse> expected = TodoResponse.MapList(mock);
            IEnumerable<TodoResponse> actual = await _todoService.GetTodos(userId);

            Assert.Equivalent(expected, actual, strict: true);
        }

        [Fact]
        public async Task CreateTodo_Success()
        {
            int userId = 1;
            var req = new TodoRequest { Title = "Test Todo 1", Completed = false };
            var mock = new Todo { Id = 1, Title = req.Title, Completed = req.Completed, UserId = userId };

            _mockTodoRepository.Setup(repo => repo.Create(It.IsAny<Todo>())).ReturnsAsync(mock);

            TodoResponse expected = TodoResponse.Map(mock);
            TodoResponse actual = await _todoService.CreateTodo(req, userId);

            Assert.Equivalent(expected, actual, strict: true);
        }

        [Fact]
        public async Task GetTodo_Success()
        {
            int todoId = 1;
            int userId = 1;
            var mock = new Todo { Id = todoId, Title = "Test Todo 1", Completed = false, UserId = userId };

            _mockTodoRepository.Setup(repo => repo.GetById(todoId)).ReturnsAsync(mock);

            TodoResponse expected = TodoResponse.Map(mock);
            TodoResponse actual = await _todoService.GetTodo(todoId, userId);

            Assert.Equivalent(expected, actual, strict: true);
        }

        [Fact]
        public async Task GetTodo_NotFound()
        {
            int nonExistId = 5000;
            int userId = 1;

            _mockTodoRepository.Setup(repo => repo.GetById(nonExistId)).ReturnsAsync(value: null);

            await Assert.ThrowsAsync<NotFoundError>(() => _todoService.GetTodo(nonExistId, userId));
        }

        [Fact]
        public async Task GetTodo_Forbidden()
        {
            int todoId = 1;
            int userId = 1;
            var otherTodo = new Todo { Id = todoId, Title = "Other's People Todo", Completed = false, UserId = 5000 };

            _mockTodoRepository.Setup(repo => repo.GetById(todoId)).ReturnsAsync(otherTodo);

            await Assert.ThrowsAsync<ForbiddenError>(() => _todoService.GetTodo(todoId, userId));
        }

        [Fact]
        public async Task UpdateTodo_Success()
        {
            int todoId = 1;
            int userId = 1;
            var req = new TodoRequest { Title = "Updated Todo", Completed = true };
            var mock = new Todo { Id = todoId, Title = req.Title, Completed = req.Completed, UserId = userId };
            var oldTodo = new Todo { Id = todoId, Title = "Old Todo", Completed = false, UserId = userId };

            _mockTodoRepository.Setup(repo => repo.GetById(todoId)).ReturnsAsync(oldTodo);
            _mockTodoRepository.Setup(repo => repo.Update(It.IsAny<Todo>())).ReturnsAsync(mock);

            TodoResponse expected = TodoResponse.Map(mock);
            TodoResponse actual = await _todoService.UpdateTodo(todoId, req, userId);

            Assert.Equivalent(expected, actual, strict: true);
        }

        [Fact]
        public async Task UpdateTodo_NotFound()
        {
            int nonExistId = 5000;
            int userId = 1;
            var req = new TodoRequest { Title = "Updated Todo", Completed = true };

            _mockTodoRepository.Setup(repo => repo.GetById(nonExistId)).ReturnsAsync(value: null);

            await Assert.ThrowsAsync<NotFoundError>(() => _todoService.UpdateTodo(nonExistId, req, userId));
        }

        [Fact]
        public async Task UpdateTodo_Forbidden()
        {
            int todoId = 1;
            int userId = 1;
            var req = new TodoRequest { Title = "Updated Todo", Completed = true };
            var otherTodo = new Todo { Id = todoId, Title = "Other's People Todo", Completed = false, UserId = 5000 };

            _mockTodoRepository.Setup(repo => repo.GetById(todoId)).ReturnsAsync(otherTodo);

            await Assert.ThrowsAsync<ForbiddenError>(() => _todoService.UpdateTodo(todoId, req, userId));
        }

        [Fact]
        public async Task DeleteTodo_Success()
        {
            int todoId = 1;
            int userId = 1;
            var mock = new Todo { Id = todoId, Title = "Test Todo 1", Completed = false, UserId = userId };

            _mockTodoRepository.Setup(repo => repo.GetById(todoId)).ReturnsAsync(mock);
            _mockTodoRepository.Setup(repo => repo.Delete(mock));

            await _todoService.DeleteTodo(todoId, userId);
        }

        [Fact]
        public async Task DeleteTodo_NotFound()
        {
            int nonExistId = 5000;
            int userId = 1;

            _mockTodoRepository.Setup(repo => repo.GetById(nonExistId)).ReturnsAsync(value: null);

            await Assert.ThrowsAsync<NotFoundError>(() => _todoService.DeleteTodo(nonExistId, userId));
        }

        [Fact]
        public async Task DeleteTodo_Forbidden()
        {
            int todoId = 1;
            int userId = 1;
            var otherTodo = new Todo { Id = todoId, Title = "Other's People Todo", Completed = false, UserId = 5000 };

            _mockTodoRepository.Setup(repo => repo.GetById(todoId)).ReturnsAsync(otherTodo);

            await Assert.ThrowsAsync<ForbiddenError>(() => _todoService.DeleteTodo(todoId, userId));
        }

    }
}