using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoRepository : BaseRepository<Todo>, ITodoRepository
    {
        private readonly DataContext _dbcontext;

        public TodoRepository(DataContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Todo>> GetAllByUserId(int userId)
        {
            return await _dbcontext.Todos.Where(t => t.UserId == userId).ToListAsync();
        }

    }
}