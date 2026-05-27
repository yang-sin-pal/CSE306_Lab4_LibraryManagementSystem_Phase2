using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public partial class Loan
{
    [Key]
    public int LoanId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required]
    public DateTime LoanDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public int Status { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
