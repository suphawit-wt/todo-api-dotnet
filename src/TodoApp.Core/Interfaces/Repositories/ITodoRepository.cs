using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces.Repositories
{
    public interface ITodoRepository : IBaseRepository<Todo>
    {
        Task<IEnumerable<Todo>> GetAllByUserId(int userId);
    }
}