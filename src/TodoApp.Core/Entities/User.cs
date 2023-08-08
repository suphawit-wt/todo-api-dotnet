using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.ValueObjects;

namespace TodoApp.Core.Entities
{
    [Table("users")]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Column("password")]
        [Required]
        [MaxLength(60)]
        public required string Password { get; set; }

        [Column("email")]
        [Required]
        [EmailAddress]
        [MaxLength(254)]
        public required string Email { get; set; }

        [Column("first_name")]
        [Required]
        [MaxLength(80)]
        public required string FirstName { get; set; }

        [Column("last_name")]
        [Required]
        [MaxLength(80)]
        public required string LastName { get; set; }

        [Column("role")]
        public UserRole Role { get; set; } = UserRole.User;

        public IEnumerable<Todo> Todos { get; } = new List<Todo>();
    }
}