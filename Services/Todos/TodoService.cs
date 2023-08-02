using Microsoft.EntityFrameworkCore;
using todo_api_sqlserver.Data;
using todo_api_sqlserver.Models;
using todo_api_sqlserver.Services.Auth;

namespace todo_api_sqlserver.Services.Todos
{
    public class TodoService : ITodoService
    {
        private readonly DataContext _dbcontext;
        private readonly IAuthService _authService;

        public TodoService(DataContext dbcontext, IAuthService authService)
        {
            _dbcontext = dbcontext;
            _authService = authService;
        }

        public async Task<List<Todo>> GetAll()
        {
            var userId = _authService.GetUserId();

            List<Todo> todos = await _dbcontext.Todo.Where(x => x.User_Id == userId).ToListAsync();

            return todos;
        }

        public async Task<Todo?> GetById(int id)
        {
            var userId = _authService.GetUserId();

            Todo? todo = await _dbcontext.Todo.FindAsync(id) ?? throw new Exception("404");

            if(todo.User_Id != userId)
            {
                throw new Exception("403");
            }

            return todo;
        }

        public async Task Create(Todo req)
        {
            var userId = _authService.GetUserId();

            var todo = new Todo
            {
                Title = req.Title,
                Is_Done = req.Is_Done,
                User_Id = userId,
            };

            await _dbcontext.Todo.AddAsync(todo);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task Update(int id, Todo req)
        {
            var userId = _authService.GetUserId();

            Todo? todo = await GetById(id) ?? throw new Exception("404");

            if (todo.User_Id != userId)
            {
                throw new Exception("403");
            }

            todo.Title = req.Title;
            todo.Is_Done = req.Is_Done;

            await _dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var userId = _authService.GetUserId();

            Todo? todo = await GetById(id) ?? throw new Exception("404");

            if (todo.User_Id != userId)
            {
                throw new Exception("403");
            }

            _dbcontext.Todo.Remove(todo);
            await _dbcontext.SaveChangesAsync();
        }

    }
}
