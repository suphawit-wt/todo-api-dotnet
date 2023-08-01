using todo_api_sqlserver.Models;

namespace todo_api_sqlserver.Services.Todos
{
    public interface ITodoService
    {
        Task<List<Todo>> GetAll();
        Task<Todo?> GetById(int id);
        Task Create(Todo req);
        Task Update(int id, Todo req);
        Task Delete(int id);
    }
}