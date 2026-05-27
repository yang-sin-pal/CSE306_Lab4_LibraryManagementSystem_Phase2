namespace LibraryManagementSystem.DTOs
{
    public class BookUpdateDto
    {
        public int BookId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? BookCode { get; set; }

        public string? Publisher { get; set; }

        public DateTime? PublishedYear { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public IFormFile? ImageFile { get; set; }

        public IFormFile? PdfFile { get; set; }
    }
}
