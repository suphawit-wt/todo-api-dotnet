using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Models.Request
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [StringLength(60)]
        public required string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public required string Email { get; set; }

        [Required]
        [StringLength(80)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(80)]
        public required string LastName { get; set; }
    }
}