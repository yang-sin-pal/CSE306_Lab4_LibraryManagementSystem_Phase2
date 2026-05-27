using System.ComponentModel.DataAnnotations;

public class AuthorCreateDto
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? Biography { get; set; }

    public string? Nationality { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public IFormFile? AvatarFile { get; set; }
}