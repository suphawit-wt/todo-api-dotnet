using TodoApp.Core.Entities;

namespace TodoApp.Core.Models.Response
{
    public class TodoResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool Completed { get; set; } = false;
        public int UserId { get; set; }

        public static TodoResponse Map(Todo todo)
        {
            return new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Completed = todo.Completed,
                UserId = todo.UserId,
            };
        }

        public static IEnumerable<TodoResponse> MapList(IEnumerable<Todo> todos)
        {
            return todos.Select(Map).ToList();
        }

    }
}