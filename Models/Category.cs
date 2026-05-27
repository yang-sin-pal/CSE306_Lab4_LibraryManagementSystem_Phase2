using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
