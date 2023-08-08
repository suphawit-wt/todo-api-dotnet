using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Core.Entities
{
    [Table("todos")]
    public class Todo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        [Column("completed")]
        public bool Completed { get; set; } = false;

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}