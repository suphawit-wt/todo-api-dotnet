using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace todo_api_sqlserver.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string First_Name { get; set; }
        [Required]
        public required string Last_Name { get; set; }
    }
}
