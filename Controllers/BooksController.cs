using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public BooksController(LibraryManagementDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAll(string? search, int? categoryId, int? authorId,int page = 1, int pageSize = 10)
        {
            var query = _context.Books
                .Where(b => !b.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.Title.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId)
                    .Include(b => b.Category);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId)
                    .Include(b => b.Author);
            }

            var books = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books
                .Where(b => !b.IsDeleted)
                .Include(b => b.Category)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound("This book isn't exist in database. Try another id.");
            }

            return Ok(new
            {
                message = "Found it!",
                data = book
            });
        }

        [HttpGet("{id}/read")]
        public async Task<IActionResult> ReadBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            if (string.IsNullOrWhiteSpace(book.Pdf))
            {
                return NotFound("This book does not have a PDF file.");
            }

            var pdfUrl = $"{Request.Scheme}://{Request.Host}{book.Pdf}";

            return Ok(new
            {
                message = "PDF found.",
                pdfUrl
            });
        }

        [HttpGet("deleted")]
        public async Task<ActionResult<List<Book>>> GetDeletedBooks()
        {
            return await _context.Books
                .Where(b => b.IsDeleted)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]BookCreateDto dto)
        {
            // Save images
            string? imagePath = null;

            if (dto.ImageFile != null)
            {
                var imageName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(dto.ImageFile.FileName);

                var fullPath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "book_images",
                        imageName);

                using var stream =
                    new FileStream(fullPath, FileMode.Create);

                await dto.ImageFile.CopyToAsync(stream);

                imagePath =
                    "/book_images/" + imageName;
            }


            // Save pdfs
            string? pdfPath = null;

            if (dto.PdfFile != null)
            {
                var pdfName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(dto.PdfFile.FileName);

                var fullPath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "book_pdfs",
                        pdfName);

                using var stream =
                    new FileStream(fullPath, FileMode.Create);

                await dto.PdfFile.CopyToAsync(stream);

                pdfPath =
                    "/book_pdfs/" + pdfName;
            }

            var newBook = new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                BookCode = dto.BookCode,
                Publisher = dto.Publisher,
                PublishedYear = dto.PublishedYear,

                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId,

                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.AvailableCopies,

                Avatar = imagePath,
                Pdf = pdfPath,

                CreatedDate = DateTime.Now,

                IsDeleted = false,
                IsActive = true
            };

            _context.Books.Add(newBook);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newBook.BookId }, newBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromForm] BookUpdateDto dto)
        {
            if (id != dto.BookId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeBook = await _context.Books.FindAsync(id);

            if (willChangeBook == null)
            {
                return NotFound("This book isn't exist in database. Try another id.");
            }

            // Replace image
            if (dto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(willChangeBook.Avatar))
                {
                    var oldImagePath =
                        Path.Combine(
                            _environment.WebRootPath,
                            willChangeBook.Avatar.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var imageName =
                    Guid.NewGuid() +
                    Path.GetExtension(dto.ImageFile.FileName);

                var newImagePath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "book_images",
                        imageName);

                using var stream =
                    new FileStream(newImagePath, FileMode.Create);

                await dto.ImageFile.CopyToAsync(stream);

                willChangeBook.Avatar =
                    "/book_images/" + imageName;
            }

            //Replace pdf
            if (dto.PdfFile != null)
            {
                if (!string.IsNullOrEmpty(willChangeBook.Pdf))
                {
                    var oldPdfPath =
                        Path.Combine(
                            _environment.WebRootPath,
                            willChangeBook.Pdf.TrimStart('/'));

                    if (System.IO.File.Exists(oldPdfPath))
                    {
                        System.IO.File.Delete(oldPdfPath);
                    }
                }

                var pdfName =
                    Guid.NewGuid() +
                    Path.GetExtension(dto.PdfFile.FileName);

                var newPdfPath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "book_pdfs",
                        pdfName);

                using var stream =
                    new FileStream(newPdfPath, FileMode.Create);

                await dto.PdfFile.CopyToAsync(stream);

                willChangeBook.Pdf =
                    "/book_pdfs/" + pdfName;
            }

            willChangeBook.Title = dto.Title;
            willChangeBook.Description = dto.Description;
            willChangeBook.BookCode = dto.BookCode;
            willChangeBook.Publisher = dto.Publisher;
            willChangeBook.PublishedYear = dto.PublishedYear;

            willChangeBook.AuthorId = dto.AuthorId;
            willChangeBook.CategoryId = dto.CategoryId;

            willChangeBook.TotalCopies = dto.TotalCopies;
            willChangeBook.AvailableCopies = dto.AvailableCopies;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This book is updated successfully",
                data = willChangeBook
            });
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            if (!book.IsDeleted)
            {
                return BadRequest("This book is not deleted.");
            }

            book.IsDeleted = false;
            book.IsActive = true;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Book restored successfully.",
                data = book
            });
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("This book isn't exist in database. Try another id.");
            }

            
            book.IsDeleted = true;
            book.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok("This book is deleted successfully.");
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            // Delete avatar file
            if (!string.IsNullOrWhiteSpace(book.Avatar))
            {
                var avatarPath = Path.Combine(
                    _environment.WebRootPath,
                    book.Avatar.TrimStart('/'));

                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }
            }

            // Delete PDF file
            if (!string.IsNullOrWhiteSpace(book.Pdf))
            {
                var pdfPath = Path.Combine(
                    _environment.WebRootPath,
                    book.Pdf.TrimStart('/'));

                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
            }

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Book permanently deleted."
            });
        }

    }
}
