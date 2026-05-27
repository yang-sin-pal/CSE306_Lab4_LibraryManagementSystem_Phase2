using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(200)]
        public string Fullname { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}