using Microsoft.EntityFrameworkCore;
using todo_api_sqlserver.Data;
using todo_api_sqlserver.Models;

namespace todo_api_sqlserver.Services.Todos
{
    public class TodoService : ITodoService
    {
        private readonly DataContext _dbcontext;

        public TodoService(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<Todo>> GetAll()
        {
            List<Todo> todos = await _dbcontext.Todo.ToListAsync();

            return todos;
        }

        public async Task<Todo?> GetById(int id)
        {
            Todo? todo = await _dbcontext.Todo.FindAsync(id) ?? throw new Exception("404");

            return todo;
        }

        public async Task Create(Todo req)
        {
            var todo = new Todo
            {
                Title = req.Title,
                Is_Done = req.Is_Done,
                User_Id = req.User_Id,
            };

            await _dbcontext.Todo.AddAsync(todo);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task Update(int id, Todo req)
        {
            Todo? todo = await GetById(id) ?? throw new Exception("404");

            todo.Title = req.Title;
            todo.Is_Done = req.Is_Done;

            await _dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Todo? todo = await GetById(id) ?? throw new Exception("404");

            _dbcontext.Todo.Remove(todo);
            await _dbcontext.SaveChangesAsync();
        }

    }
}
