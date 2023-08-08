using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsername(string username);
    }
}