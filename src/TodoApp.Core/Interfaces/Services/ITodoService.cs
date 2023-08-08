using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;

namespace TodoApp.Core.Interfaces.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoResponse>> GetTodos(int userId);
        Task<TodoResponse> GetTodo(int id, int userId);
        Task<TodoResponse> CreateTodo(TodoRequest req, int userId);
        Task<TodoResponse> UpdateTodo(int id, TodoRequest req, int userId);
        Task DeleteTodo(int id, int userId);
    }
}