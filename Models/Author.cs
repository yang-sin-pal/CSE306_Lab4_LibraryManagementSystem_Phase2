using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public partial class Author
{
    [Key]
    public int AuthorId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? Biography { get; set; }

    [StringLength(100)]
    public string? Nationality { get; set; }
    
    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Website { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
