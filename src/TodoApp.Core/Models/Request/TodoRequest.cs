using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Models.Request
{
    public class TodoRequest
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        public bool Completed { get; set; } = false;
    }
}