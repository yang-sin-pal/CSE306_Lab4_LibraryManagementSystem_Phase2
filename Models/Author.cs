using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? Biography { get; set; }

    public string? Nationality { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
