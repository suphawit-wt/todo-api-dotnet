using System.ComponentModel.DataAnnotations;

namespace todo_api_sqlserver.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required bool Is_Done { get; set; }

        public int User_Id { get; set; }
    }
}
