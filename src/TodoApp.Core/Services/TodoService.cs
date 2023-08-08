using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;

namespace TodoApp.Core.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoResponse>> GetTodos(int userId)
        {
            var todos = await _todoRepository.GetAllByUserId(userId);
            return TodoResponse.MapList(todos);
        }

        public async Task<TodoResponse> GetTodo(int id, int userId)
        {
            Todo todo = await _todoRepository.GetById(id) ?? throw new NotFoundError("Todo not found.");

            if (todo.UserId != userId)
            {
                throw new ForbiddenError("You are not owner of this Todo.");
            }

            return TodoResponse.Map(todo);
        }

        public async Task<TodoResponse> CreateTodo(TodoRequest req, int userId)
        {
            var todo = new Todo
            {
                Title = req.Title,
                Completed = req.Completed,
                UserId = userId,
            };

            Todo createdTodo = await _todoRepository.Create(todo);

            return TodoResponse.Map(createdTodo);
        }

        public async Task<TodoResponse> UpdateTodo(int id, TodoRequest req, int userId)
        {
            Todo? todo = await _todoRepository.GetById(id) ?? throw new NotFoundError("Todo not found.");

            if (todo.UserId != userId)
            {
                throw new ForbiddenError("You are not owner of this Todo.");
            }

            todo.Title = req.Title;
            todo.Completed = req.Completed;

            Todo updatedTodo = await _todoRepository.Update(todo);

            return TodoResponse.Map(updatedTodo);
        }

        public async Task DeleteTodo(int id, int userId)
        {
            Todo? todo = await _todoRepository.GetById(id) ?? throw new NotFoundError("Todo not found.");

            if (todo.UserId != userId)
            {
                throw new ForbiddenError("You are not owner of this Todo.");
            }

            await _todoRepository.Delete(todo);
        }

    }
}