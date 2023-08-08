using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Models.Request
{
    public class LoginRequest
    {
        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [StringLength(60)]
        public required string Password { get; set; }
    }
}