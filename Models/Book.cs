using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public partial class Book
{
    public int BookId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? BookCode { get; set; }

    public string? Publisher { get; set; }

    public DateTime? PublishedYear { get; set; }

    public int CategoryId { get; set; }

    public int AuthorId { get; set; }

    [Range(0, int.MaxValue)]
    public int TotalCopies { get; set; }

    [Range(0, int.MaxValue)]
    public int AvailableCopies { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Avatar { get; set; }

    public string? Pdf { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
