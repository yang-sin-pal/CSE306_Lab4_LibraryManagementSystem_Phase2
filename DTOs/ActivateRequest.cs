using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class ActivateRequest
    {
        [Required]
        public string Code { get; set; } = null!;
    }
}